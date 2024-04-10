using System;
using System.Collections.Generic;
using System.Net;
using GameLib.Network;
using UnityEngine;
using System.Linq;
using Gameplay.Data;

namespace Gameplay.Broadcaster
{
    /// <summary>
    /// 主机信息接收者。
    /// </summary>
    public class BroadcastSubscriber : MonoBehaviour
    {
        private BroadcastListener<LobbyInfo> _listener;

        private readonly Dictionary<IPAddress, LobbyInfo> _lobbyInfos = new();

        private readonly Dictionary<IPAddress, float> _lastUpdateInfos = new();

        private const float CleanInterval = 5.0f;

        /// <summary>
        /// 房间信息更新时触发。
        /// </summary>
        public event Action OnLobbyInfoUpdated;

        /// <summary>
        /// 获得所有房间信息。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<IPAddress, LobbyInfo> GetAllLobbyInfo()
        {
            return _lobbyInfos;
        }
        
        private void Start()
        {
            _listener = new();
            _listener.OnReceivedBroadcast += OnReceivedBroadcast;
            StartListening();
        }

        void StartListening()
        {
            if (_listener is null) return;
            Debug.Log("开始监听！");
            _listener.StartListen();
            InvokeRepeating(nameof(CleanInvalidAddress), 0, CleanInterval);
        }
        
        private void OnReceivedBroadcast(IPAddress ipAddress, LobbyInfo info)
        {
            var needUpdate = IsNewInfo(ipAddress, info);
            
            _lobbyInfos[ipAddress] = info;
            _lastUpdateInfos[ipAddress] = Time.time;
            
            if (needUpdate)
            {
                OnLobbyInfoUpdated?.Invoke();
            }
        }

        private bool IsNewInfo(IPAddress ipAddress, LobbyInfo info)
        {
            if (_lobbyInfos.TryGetValue(ipAddress, out LobbyInfo curInfo))
            {
                return !curInfo.Equals(info);
            }
            return true;
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void CleanInvalidAddress()
        {
            var curTime = Time.time;
            var invalidAddresses = (from pair in _lastUpdateInfos
                where curTime - pair.Value >= CleanInterval
                select pair.Key).ToList();

            foreach (var ip in invalidAddresses)
            {
                _lastUpdateInfos.Remove(ip);
                _lobbyInfos.Remove(ip);
            }

            if (invalidAddresses.Count > 0)
            {
                OnLobbyInfoUpdated?.Invoke();
            }
        }

        private void OnDisable()
        {
            if (_listener is null) return;
            Debug.Log("停止监听！");
            _listener.StopListen();
            _lastUpdateInfos.Clear();
            _lobbyInfos.Clear();
            CancelInvoke();
        }

        private void OnDestroy()
        {
            _listener?.StopListen();
            _listener?.Dispose();
        }
    }
}