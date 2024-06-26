﻿using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DeckData", menuName = "数据/牌组/牌组数据", order = 0)]
    public class DeckData : DictionaryScriptObj<CardData, int>
    {
        [Tooltip("牌组类型")]
        public Deck deckType;
        
        [Tooltip("牌背")]
        public Sprite deckBack;

        [Tooltip("玩家数量修正。")]
        public List<DeckModifier> playerModifier;
    }
}