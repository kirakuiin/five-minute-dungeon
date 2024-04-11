using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "数据/资源数据", order = 0)]
    public class ResourceData : ScriptableObject
    {
        [Tooltip("资源类型")]
        public Resource type;
        
        [Tooltip("图标")]
        public Sprite icon;
    }
}