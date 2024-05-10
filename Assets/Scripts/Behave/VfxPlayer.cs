using System.Collections;
using System.Threading.Tasks;
using Data;
using Data.Animation;
using DG.Tweening;
using GameLib.Common;
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
        
        private static readonly int Center = Shader.PropertyToID("_Center");
        
        private static readonly int BeginTime = Shader.PropertyToID("_BeginTime");

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
        
        public async Task PlayStillVfx(string vfxName, Vector3 target, float duration, bool needAwait)
        {
            PlayStillVfxRpc(vfxName, target, duration);
            if (needAwait)
            {
                await Task.Delay(TimeScalar.ConvertSecondToMs(duration));
            }
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void PlayStillVfxRpc(string vfxName, Vector3 target, float duration)
        {
            StartCoroutine(PlayStillVfxCoroutine(vfxName, target, duration));
        }

        private IEnumerator PlayStillVfxCoroutine(string vfxName, Vector3 target, float duration)
        {
            var obj = GetVfxInstance(vfxName);
            obj.transform.position = target;
            yield return new WaitForSeconds(duration);
            ReturnVfxObj(vfxName, obj);
        }

        public override void OnDestroy()
        {
            timeStop.SetActive(false);
        }
    }
}