using System.Collections.Generic;
using System.Linq;
using GameLib.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data
{
    /// <summary>
    /// 提供游戏内的数据查询服务
    /// </summary>
    public class DataService : Singleton<DataService>
    {
        private readonly Dictionary<Class, ClassData> _classData = new();

        private readonly Dictionary<Door, DoorCardData> _doorData = new();

        private readonly Dictionary<Challenge, ChallengeCardData> _challengeCard = new();
        
        private readonly Dictionary<Card, CardData> _playerCardData = new();
        
        private readonly Dictionary<Deck, DeckData> _deckData = new();
        
        private readonly Dictionary<Skill, SkillData> _skillData = new();

        private readonly Dictionary<Boss, BossData> _bossData = new();
        
        private readonly Dictionary<Resource, ResourceData> _resourceData = new();

        private int _initNum = 0;

        private const int NeedInitNum = 8;

        protected override void OnInitialized()
        {
            _initNum = 0;
            Addressables.LoadAssetsAsync<ClassData>("ClassData", OnClassLoadDone).Completed
                += handle =>
                {
                    Debug.Log("职业加载完毕");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<DoorCardData>("DoorData", OnDoorCardLoadDone).Completed
                += handle =>
                {
                    Debug.Log("门卡加载完毕");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<ChallengeCardData>("ChallengeData", OnChallengeDataLoadDone).Completed
                += handle =>
                {
                    Debug.Log("挑战卡加载完毕");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<CardData>("PlayerCardData", OnPlayerCardLoadDone).Completed
                += handle =>
                {
                    Debug.Log("玩家卡加载完毕");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<DeckData>("DeckData", OnDeckDataLoadDone).Completed
                += handle =>
                {
                    Debug.Log("牌组加载完毕。");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<SkillData>("SkillData", OnSkillDataLoadDone).Completed
                += handle =>
                {
                    Debug.Log("技能加载完毕。");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<BossData>("BossData", OnBossLoadDone).Completed
                += handle =>
                {
                    Debug.Log("boss加载完毕。");
                    _initNum += 1;
                };
            Addressables.LoadAssetsAsync<ResourceData>("ResourceData", OnResourceLoadDone).Completed
                += handle =>
                {
                    Debug.Log("resource加载完毕。");
                    _initNum += 1;
                };
        }

        public override bool IsInitialized()
        {
            return _initNum == NeedInitNum;
        }

        private void OnClassLoadDone(ClassData data)
        {
            _classData[data.classType] = data;
        }

        private void OnDoorCardLoadDone(DoorCardData data)
        {
            _doorData[data.card] = data;
        }

        private void OnChallengeDataLoadDone(ChallengeCardData data)
        {
            _challengeCard[data.card] = data;
        }
        
        private void OnPlayerCardLoadDone(CardData data)
        {
            _playerCardData[data.card] = data;
        }
        
        private void OnDeckDataLoadDone(DeckData data)
        {
            _deckData[data.deckType] = data;
        }
        
        private void OnSkillDataLoadDone(SkillData data)
        {
            _skillData[data.skill] = data;
        }
        
        private void OnBossLoadDone(BossData data)
        {
            _bossData[data.boss] = data;
        }
        
        private void OnResourceLoadDone(ResourceData data)
        {
            _resourceData[data.type] = data;
        }

        /// <summary>
        /// 获取全部的职业数据。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ClassData> GetAllClassData() => _classData.Values.ToList();

        /// <summary>
        /// 获取全部的门卡数据。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<DoorCardData> GetAllDoorData() => _doorData.Values.ToList();

        /// <summary>
        /// 获取全部的挑战卡数据。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ChallengeCardData> GetAllChallengeCard() => _challengeCard.Values.ToList();
        
        /// <summary>
        /// 获取全部的玩家卡牌数据。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<CardData> GetAllPlayerCard() => _playerCardData.Values.ToList();
        
        /// <summary>
        /// 获取全部的玩家卡组数据。
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<DeckData> GetAllDeck() => _deckData.Values.ToList();
        
        /// <summary>
        /// 根据职业枚举获得职业数据。
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public ClassData GetClassData(Class classType)
        {
            return _classData[classType];
        }

        /// <summary>
        /// 根据敌人卡获得其基本数据。
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public DictionaryScriptObj<Resource, int> GetEnemyCardData(EnemyCard card)
        {
            if (card.IsDoorCard())
            {
                return GetDoorCardData((Door)card.value);
            }
            else if (card.IsChallengeCard())
            {
                return GetChallengeCardData((Challenge)card.value);
            }
            else
            {
                return GetBossData((Boss)card.value);
            }
        }

        /// <summary>
        /// 根据门卡枚举获得门卡数据。
        /// </summary>
        /// <param name="doorType"></param>
        /// <returns></returns>
        public DoorCardData GetDoorCardData(Door doorType)
        {
            return _doorData[doorType];
        }

        /// <summary>
        /// 根据门卡枚举获得门卡数据。
        /// </summary>
        /// <param name="challengeType"></param>
        /// <returns></returns>
        public ChallengeCardData GetChallengeCardData(Challenge challengeType)
        {
            return _challengeCard[challengeType];
        }

        /// <summary>
        /// 获得玩家卡数据。
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public CardData GetPlayerCardData(Card card)
        {
            return _playerCardData[card];
        }
        
        /// <summary>
        /// 获得牌组数据。
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        public DeckData GetDeckData(Deck deck)
        {
            return _deckData[deck];
        }

        /// <summary>
        /// 获得技能数据。
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public SkillData GetSkillData(Skill skill)
        {
            return _skillData[skill];
        }

        public BossData GetBossData(Boss boss)
        {
            return _bossData[boss];
        }

        public ResourceData GetResourceData(Resource res)
        {
            return _resourceData[res];
        }
    }
}