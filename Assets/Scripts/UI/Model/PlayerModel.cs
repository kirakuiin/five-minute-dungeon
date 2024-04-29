using System.Collections.Generic;
using GameLib.Common.Extension;
using UI.Common;
using UnityEngine;

namespace UI.Model
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private List<InitComponent> compList;
        
        /// <summary>
        /// 玩家ID
        /// </summary>
        public ulong PlayerID { private set; get; }
        
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="playerID"></param>
        public void Init(ulong playerID)
        {
            PlayerID = playerID;
            compList.Apply(comp => comp.Init());
        }
    }
}