using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Animation;
using DG.Tweening;
using GameLib.Common;
using GameLib.Common.Extension;
using Gameplay.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;
using Debug = System.Diagnostics.Debug;

namespace Behave
{
    public class VfxPlayer : NetworkBehaviour, IVfxPlayer
    {
        [SerializeField] private GameObject laserVfx;

        [SerializeField] private FullScreenPassRendererFeature timeStop;
        
        [SerializeField] private FullScreenPassRendererFeature resDissolve;
        
        [SerializeField] private FullScreenPassRendererFeature classDissolve;
        
        private static readonly int Center = Shader.PropertyToID("_Center");
        
        private static readonly int BeginTime = Shader.PropertyToID("_BeginTime");
        
        private static readonly int DissolveTime = Shader.PropertyToID("_DissolveTime");
        
        private static readonly int ResIndex = Shader.PropertyToID("_ResIndex");
        
        private static readonly int Texture = Shader.PropertyToID("_Texture");

        private void Start()
        {
            InitListen();
        }

        private void InitListen()
        {
            GamePlayContext.Instance.GetTimeRuntimeInfo().OnTimeIsFlow += OnTimeIsFlow;
        }

        private void OnTimeIsFlow(bool isFlow)
        {
            timeStop.SetActive(false);
        }

        public async Task PlayLaser(Vector3 from, Vector3 to, float duration, Color color, Color subColor)
        {
            PlayLaserRpc(from, to, duration, color, subColor);
            await Task.CompletedTask;
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayLaserRpc(Vector3 from, Vector3 to, float duration, Color color, Color subColor)
        {
            var obj = GameObjectPool.Instance.Get(laserVfx);
            var direction = (to - from);
            obj.transform.SetParent(transform);
            obj.transform.position = from;
            obj.transform.forward = direction;
            InitVfx(obj.GetComponent<VisualEffect>(), duration,direction.magnitude, color, subColor);
            StartCoroutine(ReturnObj(duration, obj, laserVfx));
        }

        private void InitVfx(VisualEffect vfx, float duration, float length, Color color, Color subColor)
        {
            vfx.Reinit();
            vfx.SetFloat("duration", duration);
            vfx.SetFloat("length", length);
            vfx.SetVector4("laserColor", color);
            vfx.SetVector4("subColor", subColor);
            vfx.Play();
        }

        private static IEnumerator ReturnObj(float delay, GameObject obj, GameObject prefab)
        {
            yield return new WaitForSeconds(delay);
            GameObjectPool.Instance.ReturnWithReParent(obj, prefab);
        }
        
        public async Task TimeStop(Vector3 center)
        {
            TimeStopRpc(center);
            await Task.CompletedTask;
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void TimeStopRpc(Vector3 center)
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            var pos = Camera.main.WorldToScreenPoint(center);
            var screenCenter = new Vector2(pos.x/Screen.width, pos.y/Screen.height);
            timeStop.passMaterial.SetVector(Center, screenCenter);
            timeStop.passMaterial.SetFloat(BeginTime, Time.time);
            timeStop.SetActive(true);
        }
        
        public async Task PlayProjectile(string vfxName, Vector3 from, Vector3 to, float duration)
        {
            PlayProjectileRpc(vfxName, from, to, duration);
            await Task.Delay(TimeScalar.ConvertSecondToMs(duration));
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayProjectileRpc(string vfxName, Vector3 from, Vector3 to, float duration)
        {
            var obj = GetVfxInstance(vfxName);
            obj.transform.position = from;
            obj.transform.right = (to - from);
            obj.transform.DOMove(to, duration).OnComplete(() => ReturnVfxObj(vfxName, obj));
        }

        private void ReturnVfxObj(string vfxName, GameObject obj)
        {
            var info = DataService.Instance.GetVfxData(vfxName);
            GameObjectPool.Instance.ReturnWithReParent(obj, info.prefab);
        }

        private GameObject GetVfxInstance(string vfxName)
        {
            var info = DataService.Instance.GetVfxData(vfxName);
            if (info.isParticleSystem)
            {
                info.prefab.GetComponent<ParticleSystem>().Play();
            }
            var obj = GameObjectPool.Instance.Get(info.prefab);
            obj.transform.SetParent(transform);
            return obj;
        }
        
        public async Task PlayStillVfx(string vfxName, StillVfxParam param)
        {
            PlayStillVfxRpc(vfxName, param);
            if (param.needAwait)
            {
                await Task.Delay(TimeScalar.ConvertSecondToMs(param.duration));
            }
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayStillVfxRpc(string vfxName, StillVfxParam param)
        {
            StartCoroutine(PlayStillVfxCoroutine(vfxName, param));
        }

        private IEnumerator PlayStillVfxCoroutine(string vfxName, StillVfxParam param)
        {
            var obj = GetVfxInstance(vfxName);
            obj.transform.position = param.target;
            obj.transform.rotation = Quaternion.Euler(param.rotation);
            var info = DataService.Instance.GetVfxData(vfxName);
            if (info.isParticleSystem)
            {
                ApplyParamToAllChild(obj, param);
            }
            yield return new WaitForSeconds(param.duration);
            ReturnVfxObj(vfxName, obj);
        }

        private void ApplyParamToAllChild(GameObject particle, StillVfxParam param)
        {
            var main = particle.GetComponent<ParticleSystem>().main;
            main.simulationSpeed = param.speed;
            Enumerable.Range(0, particle.transform.childCount).Apply(
                i =>
                {
                    var subParticle = particle.transform.GetChild(i).GetComponent<ParticleSystem>();
                    if (!subParticle) return;
                    var subMain = subParticle.main;
                    subMain.simulationSpeed = param.speed;
                });
        }

        public async Task PlayDissolveRes(Resource res)
        {
            PlayDissolveResRpc(res);
            await Task.CompletedTask;
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void PlayDissolveResRpc(Resource res)
        {
            resDissolve.passMaterial.SetFloat(BeginTime, Time.time);
            resDissolve.passMaterial.SetFloat(ResIndex, (int)res);
            resDissolve.SetActive(true);
            StartCoroutine(StopDissolveCoroutine());
        }
        
        private IEnumerator StopDissolveCoroutine()
        {
            yield return new WaitForSeconds(classDissolve.passMaterial.GetFloat(DissolveTime));
            resDissolve.SetActive(false);
        }

        public async Task PlayDissolveClass(Class clsType)
        {
            PlayDissolveClassRpc(clsType);
            await Task.CompletedTask;
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayDissolveClassRpc(Class clsType)
        {
            classDissolve.passMaterial.SetFloat(BeginTime, Time.time);
            classDissolve.passMaterial.SetTexture(Texture,
                DataService.Instance.GetClassData(clsType).portraits.texture);
            classDissolve.SetActive(true);
            StartCoroutine(StopDissolveClassCoroutine());
        }
        
        private IEnumerator StopDissolveClassCoroutine()
        {
            yield return new WaitForSeconds(classDissolve.passMaterial.GetFloat(DissolveTime));
            classDissolve.SetActive(false);
        }

        public override void OnDestroy()
        {
            timeStop.SetActive(false);
            resDissolve.SetActive(false);
            classDissolve.SetActive(false);
        }
    }
}