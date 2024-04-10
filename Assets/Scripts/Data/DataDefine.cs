using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Data
{
    /// <summary>
    /// 职业类型。
    /// </summary>
    public enum Class
    {
        Wizard,
        Sorceress,
        Thief,
        Ninja,
        Paladin,
        Valkyrie,
        Barbarian,
        Gladiator,
        Ranger,
        Huntress,
    }

    /// <summary>
    /// 技能类型。
    /// </summary>
    public enum Skill
    {
        TimeStop,
        Teleport,
        PickPocket,
        Vault,
        Smite,
        Inspire,
        Slay,
        Intimidate,
        TrickShot,
        AnimalCompanion,
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum Resource
    {
        Arrow,
        Jump,
        Sword,
        Shield,
        Scroll,
        Wild,
    }

    /// <summary>
    /// 卡组类型。
    /// </summary>
    public enum Deck
    {
        Blue,
        Purple,
        Yellow,
        Red,
        Green,
    }

    /// <summary>
    /// 卡牌种类。
    /// </summary>
    public enum Card
    {
        Cancel,
        MagicBomb,
        Fireball,
        
        BackStab,
        Donation,
        Sprint,
        Steal,
        
        Heal,
        DivineShield,
        HolyGrenade,
        HealthPotion,
        Smite,
        
        Engage,
        MightyLeap,
        
        Snipe,
        WildCard,
        HealingHerbs,
        
        Arrow,
        Jump,
        Scroll,
        Shield,
        Sword,
        SwordArrow,
        SwordJump,
        SwordShield,
        SwordScroll,
        TwoArrow,
        TwoJump,
        TwoScroll,
        TwoShield,
        TwoSword,
    }

    /// <summary>
    /// 卡牌分类。
    /// </summary>
    public enum PlayerCardType
    {
        ActionCard,
        ResourceCard,
    }
    
    /// <summary>
    /// boss枚举
    /// </summary>
    public enum Boss
    {
        BabyBarbarian,
        TheGrimReaper,
        ZolaTheGorgon,
        AFreakingDragon,
        TheDungeonMaster,
        TheKick9000,
        FinalForm,
    }

    /// <summary>
    /// 门卡枚举。
    /// </summary>
    public enum Door
    {
        AWarriorPrincess,
        TheNecromancer,
        Steve,
        SquireSoldier,
        ASleepingGiant,
        ScreamingChildren,
        APuppetShow,
        AMerchant,
        MassiveCloak,
        TheTall,
        AGhost,
        RichMan,
        BarberArian,
        AnArmDealer,
        Ninja,
        Dwarfs,
        OneBow,
        TwoBow,
        
        WallOfSpike,
        WallOfIce,
        ATournament,
        Shortcut,
        Quicksand,
        LoadingScreen,
        LivingVines,
        AScarecrow,
        JackBox,
        InvisibleWall,
        DisappearingBlock,
        ADeadlyGame,
        CollapseCeiling,
        IronThrone,
        TheCarpalTunnel,
        ABunchOfStair,
        BottomlessPit,
        TrappedChest,
        
        Boots,
        EvilCreature,
        TimberWolf,
        TheUnicorn,
        ASuspiciousCrate,
        AStraightGhost,
        FuzzyLumps,
        SharkWithLegs,
        AStoneStatue,
        Zombies,
        Lanterns,
        AGriffin,
        Goblin,
        ARockyFairy,
        Err,
        TheDuckOfCanterbury,
        ACactus,
        AdorableSlime,
        APheasant,
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
    }
    
    /// <summary>
    /// 挑战卡枚举。
    /// </summary>
    public enum Challenge
    {
        LockedDoor,
        TrapDoor,
        Porcupines,
        SuddenIllness,
        YetMoreSpikes,
        GimmeAHand,
        DungeonError,
        Confusion,
        ABooBoo,
        Ambush,
        
        TheRatKing,
        TheVeryMiniBoss,
        AWizardOfIllness,
        APython,
        ALowTechRobot,
        DasBoot,
        TheCollector,
        TheGoblinKing,
        GiantEnemyGrab,
        TheTriBread,
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
    /// 敌方卡牌通用接口。
    /// </summary>
    public interface IEnemyCard
    {
        /// <summary>
        /// 卡牌类型。
        /// </summary>
        public EnemyCardType Type { get; }

        /// <summary>
        /// 获得击败敌人所需要的全部资源。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Resource> GetAllNeedResource();
        
        public int CardValue { get; }
        
        public string ToReadableString()
        {
            var builder = new StringBuilder();
            builder.Append($"{Type}[");
            foreach (var resource in GetAllNeedResource())
            {
                builder.Append($"{resource}, ");
            }
            builder.Append("]");
            
            return builder.ToString();
        }
    }
}

