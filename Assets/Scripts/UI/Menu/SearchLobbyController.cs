using System;
using System.Net;
using GameLib.Common;
using GameLib.Common.Extension;
using Gameplay.Broadcaster;
using Gameplay.Data;
using UnityEngine;

namespace UI.Menu
{
    public class SearchLobbyController : MonoBehaviour
    {
        [SerializeField]
        private BroadcastSubscriber subscriber;

        [SerializeField]
        private Transform scroll;

        [SerializeField]
        private GameObject itemPrefab;

        [SerializeField]
        private SearchJoinController controller;
        

        private void Start()
        {
            subscriber.OnLobbyInfoUpdated += OnLobbyInfoUpdated;
            FillLobbyInfo();
        }

        private void OnDestroy()
        {
            subscriber.OnLobbyInfoUpdated -= OnLobbyInfoUpdated;
        }

        private void OnLobbyInfoUpdated()
        {
            FillLobbyInfo();
        }

        private void FillLobbyInfo()
        {
            CleanScroll();
            foreach (var pair in subscriber.GetAllLobbyInfo())
            {
                var instance = GameObjectPool.Instance.Get(itemPrefab);
                instance.GetComponent<LobbyInfoItemController>().Init(pair.Key, pair.Value, OnItemClick);
                instance.transform.SetParent(scroll);
            }
        }

        private void OnItemClick(IPAddress ipAddress, LobbyInfo info)
        {
            controller.Init(ipAddress.ToString(), info.lobbyName.ToString());
        }

        private void CleanScroll()
        {
            scroll.DoSomethingToAllChildren(
                obj =>
                {
                    if (obj.activeSelf)
                    {
                        GameObjectPool.Instance.ReturnWithReParent(obj, itemPrefab);
                    }
                }
            );
        }

        public void OnRefresh()
        {
            FillLobbyInfo();
        }
    }
}