using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Common.Extension;
using UnityEngine;
using XNode;

namespace Data.Animation.Nodes
{
    /// <summary>
    /// 资源颜色混合器
    /// </summary>
    public class ResColorMixerCmd : AnimationBase
    {
        public List<Resource> resList;

        [Output] public Color color;

        [Output] public Color subColor;
        
        public override async Task Execute(IBehaveController controller, AnimContext animContext)
        {
            switch (resList.Count)
            {
                case 0:
                    color = subColor = Color.black;
                    break;
                case 1:
                    (color, subColor) = (GetColor(resList[0]), Color.clear);
                    break;
                default:
                    (color, subColor) = (GetColor(resList[0]), GetColor(resList[1]));
                    break;
            }
            await Task.CompletedTask;
        }

        private Color GetColor(Resource res)
        {
            return DataService.Instance.GetResourceData(res).color;
        }

        public override object GetValue(NodePort port)
        {
            return port.fieldName == nameof(color) ? color : subColor;
        }
    }
}