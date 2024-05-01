using System.Collections.Generic;
using Data;
using GameLib.Common.Extension;
using UI.Common;
using UnityEngine;

namespace UI.Model
{
    /// <summary>
    /// 敌方模型的管理类，负责控制其他组件。
    /// </summary>
    public class EnemyModel : MonoBehaviour
    {
        public EnemyCard Card { private set; get; }

        public ulong EnemyID { private set; get; }

        [SerializeField]
        private List<InitComponent> compList;
        
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="enemyID"></param>
        /// <param name="card"></param>
        public void Init(ulong enemyID, EnemyCard card)
        {
            EnemyID = enemyID;
            Card = card;
            compList.Apply(comp => comp.Init());
        }

        /// <summary>
        /// 仅以模型的方式初始化。
        /// </summary>
        public void InitAsPureModel()
        {
            compList.Apply(obj => obj.enabled = false);
        }
    }
}