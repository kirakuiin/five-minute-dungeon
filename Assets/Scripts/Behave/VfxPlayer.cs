using System.Collections;
using System.Threading.Tasks;
using Data.Animation;
using GameLib.Common;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

namespace Behave
{
    public class VfxPlayer : NetworkBehaviour, IVfxPlayer
    {
        [SerializeField] private GameObject laserVfx;
        
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
    }
}