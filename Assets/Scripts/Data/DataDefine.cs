using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Data
{
    /// <summary>
    /// 职业类型。
    /// </summary>
    public enum Class
    {
        Wizard=0,
        Sorceress=1,
        Thief=2,
        Ninja=3,
        Paladin=4,
        Valkyrie=5,
        Barbarian=6,
        Gladiator=7,
        Ranger=8,
        Huntress=9,
    }

    /// <summary>
    /// 技能类型。
    /// </summary>
    public enum Skill
    {
        TimeStop=0,
        Teleport=1,
        PickPocket=2,
        Vault=3,
        Smite=4,
        Inspire=5,
        Slay=6,
        Intimidate=7,
        TrickShot=8,
        AnimalCompanion=9,
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum Resource
    {
        Arrow=0,
        Jump=1,
        Sword=2,
        Shield=3,
        Scroll=4,
        Wild=5,
    }

    /// <summary>
    /// 卡组类型。
    /// </summary>
    public enum Deck
    {
        Blue=0,
        Purple=1,
        Yellow=2,
        Red=3,
        Green=4,
    }

    /// <summary>
    /// 卡牌种类。
    /// </summary>
    public enum Card
    {
        Cancel=0,
        MagicBomb=1,
        Fireball=2,
        
        BackStab=3,
        Donation=4,
        Sprint=5,
        Steal=6,
        
        Heal=7,
        DivineShield=8,
        HolyGrenade=9,
        HealthPotion=10,
        Smite=11,
        
        Engage=12,
        MightyLeap=13,
        
        Snipe=14,
        WildCard=15,
        HealingHerbs=16,
        
        Arrow=17,
        Jump=18,
        Scroll=19,
        Shield=20,
        Sword=21,
        SwordArrow=22,
        SwordJump=23,
        SwordShield=24,
        SwordScroll=25,
        TwoArrow=26,
        TwoJump=27,
        TwoScroll=28,
        TwoShield=29,
        TwoSword=30,
    }

    /// <summary>
    /// 卡牌分类。
    /// </summary>
    public enum PlayerCardType
    {
        ActionCard=0,
        ResourceCard=1,
    }
    
    /// <summary>
    /// boss枚举
    /// </summary>
    public enum Boss
    {
        BabyBarbarian=0,
        TheGrimReaper=1,
        ZolaTheGorgon=2,
        AFreakingDragon=3,
        TheDungeonMaster=4,
        TheKick9000=5,
        FinalForm=6,
    }

    /// <summary>
    /// 门卡枚举。
    /// </summary>
    public enum Door
    {
        AWarriorPrincess=0,
        TheNecromancer=1,
        Steve=2,
        SquireSoldier=3,
        ASleepingGiant=4,
        ScreamingChildren=5,
        APuppetShow=6,
        AMerchant=7,
        MassiveCloak=8,
        TheTall=9,
        AGhost=10,
        RichMan=11,
        BarberArian=12,
        AnArmDealer=13,
        Ninja=14,
        Dwarfs=15,
        OneBow=16,
        TwoBow=17,
        
        WallOfSpike=18,
        WallOfIce=19,
        ATournament=20,
        Shortcut=21,
        Quicksand=22,
        LoadingScreen=23,
        LivingVines=24,
        AScarecrow=25,
        JackBox=26,
        InvisibleWall=27,
        DisappearingBlock=28,
        ADeadlyGame=29,
        CollapseCeiling=30,
        IronThrone=31,
        TheCarpalTunnel=32,
        ABunchOfStair=33,
        BottomlessPit=34,
        TrappedChest=35,
        
        Boots=36,
        EvilCreature=37,
        TimberWolf=38,
        TheUnicorn=39,
        ASuspiciousCrate=40,
        AStraightGhost=41,
        FuzzyLumps=42,
        SharkWithLegs=43,
        AStoneStatue=44,
        Zombies=45,
        Lanterns=46,
        AGriffin=47,
        Goblin=48,
        ARockyFairy=49,
        Err=50,
        TheDuckOfCanterbury=51,
        ACactus=52,
        AdorableSlime=53,
        APheasant=54,
    }

    /// <summary>
    /// 敌方卡类型。
    /// </summary>
    [Flags]
    public enum EnemyCardType
    {
        Person = 1,
        Obstacle = 2,
        Monster = 4,
        Event = 8,
        MiniBoss = 16,
        Boss = 32,
    }
    
    /// <summary>
    /// 挑战卡枚举。
    /// </summary>
    public enum Challenge
    {
        LockedDoor=0,
        TrapDoor=1,
        Porcupines=2,
        SuddenIllness=3,
        YetMoreSpikes=4,
        GimmeAHand=5,
        DungeonError=6,
        Confusion=7,
        ABooBoo=8,
        Ambush=9,
        
        TheRatKing=10,
        TheVeryMiniBoss=11,
        AWizardOfIllness=12,
        APython=13,
        ALowTechRobot=14,
        DasBoot=15,
        TheCollector=16,
        TheGoblinKing=17,
        GiantEnemyGrab=18,
        TheTriBread=19,
    }

    public struct EnemyCard
    {
        public EnemyCardType type;

        public int value;

        /// <summary>
        /// 是否为门卡类型。
        /// </summary>
        /// <returns></returns>
        public bool IsDoorCard()
        {
            return ((EnemyCardType.Monster|EnemyCardType.Person|EnemyCardType.Obstacle) & type) > 0;
        }

        /// <summary>
        /// 是否为挑战卡类型。
        /// </summary>
        /// <returns></returns>
        public bool IsChallengeCard() => !IsDoorCard() && !IsBossCard();

        /// <summary>
        /// 是否为Boss
        /// </summary>
        /// <returns></returns>
        public bool IsBossCard() => type == EnemyCardType.Boss;

        /// <summary>
        /// 是否为事件卡。
        /// </summary>
        /// <returns></returns>
        public bool IsEventCard() => type == EnemyCardType.Event;
    }

    public abstract class DictionaryScriptObj<TK, TV> : ScriptableObject
    {
        [Tooltip("所需类型列表")]
        public List<TK> needTypeList;

        [Tooltip("类型值列表")]
        public List<TV> needValueList;

        private readonly Dictionary<TK, TV> _dictionary = new();
        
        /// <summary>
        /// 获得键所对应的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TV Get(TK key)
        {
            if (_dictionary.Count == 0)
            {
                BuildDictionary();
            }

            return _dictionary[key];
        }

        private void BuildDictionary()
        {
            Assert.AreEqual(needTypeList.Count, needValueList.Count, "两个列表配置数量必须一致！");
            _dictionary.Clear();
            for (var i = 0; i < needTypeList.Count; ++i)
            {
                _dictionary[needTypeList[i]] = needValueList[i];
            }
        }
    }

    /// <summary>
    /// 敌方数据配置的抽象类。
    /// </summary>
    public abstract class EnemyScriptObj : DictionaryScriptObj<Resource, int>
    {
        [Tooltip("描述")] public string desc;

        [Tooltip("模型")] public GameObject prefab;
    }
}

