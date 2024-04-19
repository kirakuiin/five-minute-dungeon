using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Check;
using GameLib.Common;
using GameLib.Common.DataStructure;
using Gameplay.Core;
using Unity.Mathematics;
using UnityEngine;

namespace UI.Gameplay
{
    public abstract class ResourcePoolUIController : MonoBehaviour
    {
        [SerializeField] protected GameObject resourcePrefab;

        [SerializeField] protected Transform layout;

        protected ILevelRuntimeInfo LevelInfo { private set; get; }
        
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Init()
        {
            LevelInfo = GamePlayContext.Instance.GetLevelRuntimeInfo();
            InitConfig();
        }

        protected virtual void InitConfig()
        {
            LevelInfo.OnResourceAdded += OnResourceAdded;
        }

        protected abstract void OnResourceAdded(Resource res, int num);
        
        protected virtual void OnDestroy()
        {
            if (LevelInfo != null)
            {
                LevelInfo.OnResourceAdded -= OnResourceAdded;
            }
        }

        protected virtual void CleanAll()
        {
            foreach (var obj in Enumerable.Range(0, layout.childCount).Select(idx => layout.GetChild(idx)).ToList())
            {
                GameObjectPool.Instance.ReturnWithReParent(obj.gameObject, resourcePrefab);
            }
        }
        
        protected ResIconControllerUI CreateIcon(Resource type)
        {
            var icon = GameObjectPool.Instance.Get(resourcePrefab).GetComponent<ResIconControllerUI>();
            icon.Init(type);
            icon.transform.SetParent(layout);
            return icon;
        }
    }
    
    /// <summary>
    /// 击败敌人所需资源池。
    /// </summary>
    public class NeedResourcePoolUIController : ResourcePoolUIController
    {
        private readonly DefaultDict<Resource, List<ResIconControllerUI>> _icons = new(() => new List<ResIconControllerUI>());

        private List<Resource> _allNeedResourceList;

        protected override void OnResourceAdded(Resource res, int num)
        {
            SetResourceIconGrey();
        }

        private void SetResourceIconGrey()
        {
            var counter = new Counter<Resource>(LevelInfo.GetAlreadyPlayedResources());
            foreach (var pair in counter)
            {
                var maxNum = math.min(_icons[pair.Key].Count, pair.Value);
                for (var i = 0; i < maxNum; ++i)
                {
                    _icons[pair.Key][i].SetGrey();
                }
            }
        }

        public void RefreshUI()
        {
            CleanAll();
            _allNeedResourceList = new List<Resource>(LevelInfo.GetCurNeedResources());
            foreach (var resource in _allNeedResourceList)
            {
                var icon = CreateIcon(resource);
                _icons[resource].Add(icon);
            }
        }

        protected override void CleanAll()
        {
            _icons.Clear();
            base.CleanAll();
        }
    }
}