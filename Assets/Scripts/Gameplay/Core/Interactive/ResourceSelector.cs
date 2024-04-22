using System;
using System.Threading.Tasks;
using Data;
using Data.Check;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 敌人选择器。
    /// </summary>
    public class ResourceSelector : NetworkBehaviour, IResourceSelector
    {
        /// <summary>
        /// 通知选择敌对单位。
        /// </summary>
        public event Action OnResourceSelecting;

        private bool _isSelect;

        private Resource _res;

        /// <summary>
        /// 选择指定资源。
        /// </summary>
        /// <param name="res"></param>
        public void SelectResource(Resource res)
        {
            SelectResourceServerRpc(res);
        }

        [Rpc(SendTo.Server)]
        private void SelectResourceServerRpc(Resource res)
        {
            _res = res;
            _isSelect = true;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<Resource> GetSelectRes()
        {
            _isSelect = false;
            GetSelectResClientRpc();
            await TaskExtension.Wait(() => _isSelect);
            return _res;
        }

        [Rpc(SendTo.Owner)]
        private void GetSelectResClientRpc()
        {
            OnResourceSelecting?.Invoke();
        }
    }
}