using UnityEngine;

namespace UI.Model
{
    public class PlayerModel : MonoBehaviour
    {
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
            GetComponent<PlayerModelUIController>().Init();
        }
    }
}