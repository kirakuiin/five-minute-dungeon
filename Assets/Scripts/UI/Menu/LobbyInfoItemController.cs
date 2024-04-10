using System;
using System.Net;
using Common;
using GameLib.Common;
using GameLib.Common.Extension;
using GameLib.Network;
using GameLib.Network.Analysis;
using GameLib.Network.NGO.ConnectionManagement;
using Gameplay.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class LobbyInfoItemController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text lobbyNameText;

        [SerializeField]
        private TMP_Text playerNumText;

        [SerializeField]
        private TMP_Text lobbyStateText;

        [SerializeField]
        private TMP_Text lobbyLatency;

        [SerializeField]
        private Button selectButton;

        private IPEndPoint _endPoint;

        private LobbyInfo _info;

        private Latency _latency;

        private Action<IPAddress, LobbyInfo> _callback;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="ipAddress">主机ip地址</param>
        /// <param name="info">房间信息</param>
        public void Init(IPAddress ipAddress, LobbyInfo info, Action<IPAddress, LobbyInfo> callback)
        {
            _endPoint = Address.GetIPEndPoint(ipAddress.ToString(), NetworkDefines.Port);
            _info = info;
            _callback = callback;
            SetStateText();
            SetNumText();
            SetLobbyNameText();
            SetPingText();
        }

        private void SetStateText()
        {
            if (_info.state == LobbyState.InGame)
            {
                lobbyStateText.text = "游戏中".ToRichText(Color.red);
            }
            else
            {
                lobbyStateText.text = "等待中".ToRichText(Color.green);
            }
        }

        private void SetNumText()
        {
            var factor = (float)_info.playerNum / ConnectionManager.Instance.config.maxConnectedPlayerNum;
            var text = $"{_info.playerNum}/{ConnectionManager.Instance.config.maxConnectedPlayerNum}";
            playerNumText.text = GetGreenRedColor(text, 0.5f);
        }

        private void SetLobbyNameText()
        {
            lobbyNameText.text = _info.lobbyName.ToString();
        }

        private async void SetPingText()
        {
            _latency = new Latency(_endPoint.Address.ToString(), NetworkDefines.Timeout);
            var result = await _latency.GetLatencyAsync();
            if (result.IsReachable())
            {
                lobbyLatency.text = GetGreenRedColor(result.Latency.ToString(),
                    (float)result.Latency / TimeScalar.ConvertSecondToMs(NetworkDefines.Timeout));
            }
            else
            {
                lobbyLatency.text = "不可达".ToRichText(Color.grey);
            }
        }

        private string GetGreenRedColor(string text, float factor)
        {
            return text.ToRichText(Color.Lerp(Color.green, Color.red, factor));
        }
        
        public void OnClick()
        {
            _callback?.Invoke(_endPoint.Address, _info);
        }
    }
}