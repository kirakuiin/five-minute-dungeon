using Data;
using Gameplay.Data;
using Save;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class ClassSelectionController : NetworkBehaviour
    {
        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Image portraitsImage;

        [SerializeField]
        private TMP_Text classNameInput;

        [SerializeField]
        private TMP_InputField playerNameInput;

        [SerializeField]
        private Button changeHeroBtn;

        [SerializeField]
        private Toggle readyToggle;

        [SerializeField]
        private GameObject classGrid;

        private readonly NetworkVariable<PlayerInfo> _curInfo = new();

        /// <summary>
        /// 设置当前玩家信息。
        /// </summary>
        /// <param name="info"></param>
        public void SetPlayerInfo(PlayerInfo info)
        {
            if (!IsSpawned) return;
            _curInfo.Value = info;
        }

        public override void OnNetworkSpawn()
        {
            InitNetworkVariable();
            SetUIInteract();
            InitPlayerName();
            Init(_curInfo.Value);
            _curInfo.OnValueChanged += OnValueChanged;
        }

        private void OnValueChanged(PlayerInfo old, PlayerInfo newInfo)
        {
            Debug.Log($"客户端{NetworkManager.LocalClientId}收到{newInfo}");
            Init(newInfo);
        }

        private void InitNetworkVariable()
        {
            if (IsServer)
            {
                _curInfo.Value = LobbyInfoData.Instance.PlayerInfos[NetworkManager.LocalClientId];
            }
        }

        private void SetUIInteract()
        {
            playerNameInput.interactable = IsOwner;
            changeHeroBtn.gameObject.SetActive(IsOwner);
            readyToggle.interactable = IsOwner;
        }

        private void Init(PlayerInfo info)
        {
            playerNameInput.text = info.PlayerName;
            InitClassView(DataService.Instance.GetClassData(info.SelectedClass));
            readyToggle.SetIsOnWithoutNotify(info.IsReady);
        }

        private void InitPlayerName()
        {
            if (!IsOwner) return;
            playerNameInput.text = PlayerSetting.Instance.PlayerName;
            LobbyInfoData.Instance.SetPlayerName(PlayerSetting.Instance.PlayerName);
        }

        public void OnClassSelected(ClassData data)
        {
            InitClassView(data);
            LobbyInfoData.Instance.SetPlayerClass(data.classType);
        }
        
        private void InitClassView(ClassData data)
        {
            backgroundImage.sprite = data.deckData.deckBack;
            portraitsImage.sprite = data.portraits;
            classNameInput.text = data.className;
        }

        public void OnChangeHero()
        {
            classGrid.SetActive(true);
        }

        public void OnPlayerNameChanged(string playerName)
        {
            LobbyInfoData.Instance.SetPlayerName(playerName);
            PlayerSetting.Instance.PlayerName = playerName;
        }

        public void OnToggleReady(bool isReady)
        {
            LobbyInfoData.Instance.SetPlayerReady(isReady);
            SwitchBtnState(!isReady);
        }

        private void SwitchBtnState(bool isEnable)
        {
            changeHeroBtn.GetComponent<CanvasGroup>().alpha = isEnable ? 1 : 0;
            changeHeroBtn.interactable = isEnable;
        }

        public override void OnNetworkDespawn()
        {
            _curInfo.OnValueChanged -= OnValueChanged;
        }
    }
}