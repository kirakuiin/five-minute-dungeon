using System;
using System.Threading.Tasks;
using Common;
using Data;
using GameLib.Common.Extension;
using Unity.Netcode;

namespace Gameplay.Core.Interactive
{
    /// <summary>
    /// 敌人选择器。
    /// </summary>
    public class ResourceSelector : Selector
    {
        /// <summary>
        /// 通知选择敌对单位。
        /// </summary>
        public event Action OnSelectResource;

        private readonly NetworkVariable<Resource> _resource =
            new NetworkVariable<Resource>(writePerm: NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<bool> _isSelect =
            new NetworkVariable<bool>(false, writePerm: NetworkVariableWritePermission.Owner);

        /// <summary>
        /// 选择指定资源。
        /// </summary>
        /// <param name="res"></param>
        public void SelectResource(Resource res)
        {
            _isSelect.Value = true;
            _resource.Value = res;
        }
        
        /// <summary>
        /// 异步通知开始进行选择。
        /// </summary>
        /// <returns></returns>
        public async Task<Resource> GetSelectRes()
        {
            GetSelectResClientRpc(GetClientParam());
            await TaskExtension.Wait(() => _isSelect.Value);
            return _resource.Value;
        }

        [ClientRpc]
        private void GetSelectResClientRpc(ClientRpcParams param = default)
        {
            _isSelect.Value = false;
            OnSelectResource?.Invoke();
        }
    }
}