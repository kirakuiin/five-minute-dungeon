using System.Collections.Generic;
using System.Linq;
using Data;
using GameLib.Common.Extension;
using Unity.Mathematics;
using Random = System.Random;

namespace Gameplay.Core
{
    /// <summary>
    /// 牌组生成器。
    /// </summary>
    public class DeckGenerator
    {
        private readonly List<Class> _classes;

        private readonly List<List<Card>> _decks = new();

        private readonly HashSet<Deck> _allDecks = new() { Deck.Blue, Deck.Green, Deck.Red, Deck.Purple, Deck.Yellow };

        public DeckGenerator(IEnumerable<Class> eachPlayerClass)
        {
            _classes = new List<Class>(eachPlayerClass);
            Generate();
        }

        private void Generate()
        {
            var playerCount = _classes.Count;
            foreach (var cls in _classes)
            {
                var deck = GetDeckByClass(cls);
                _decks.Add(new List<Card>(GenerateCardsByDeck(deck)));
                ApplyModifier(_decks.Last(), cls, playerCount);
            }
            AddAdditionalDeck(playerCount);
            ShuffleAll();
        }

        private void ApplyModifier(ICollection<Card> deck, Class cls, int playerCount)
        {
            foreach (var modifier in DataService.Instance.GetDeckData(GetDeckByClass(cls)).playerModifier)
            {
                if (modifier.playerNum != playerCount) continue;
                foreach (var card in modifier.needTypeList)
                {
                    var cardNum = modifier.Get(card);
                    for (var i = 0; i < math.abs(cardNum); ++i)
                    {
                        if (cardNum > 0)
                        {
                            deck.Add(card);
                        }
                        else
                        {
                            deck.Remove(card);
                        }
                    }
                }
            }
        }

        private void ShuffleAll()
        {
            var random = new Random();
            foreach (var deck in _decks)
            {
                random.Shuffle(deck);
            }
        }

        private void AddAdditionalDeck(int playerCount)
        {
            if (playerCount > 2) return;
            var curDecks = new HashSet<Deck>(from cls in _classes
                select GetDeckByClass(cls));
            var random = new Random();
            var anotherDecks = random.Sample(_allDecks.Except(curDecks), _classes.Count);
            for (var i = 0; i < _decks.Count; ++i)
            {
                _decks[i].AddRange(GenerateCardsByDeck(anotherDecks[i]));
            }
        }

        private Deck GetDeckByClass(Class cls)
        {
            return DataService.Instance.GetClassData(cls).deckData.deckType;
        }

        private IEnumerable<Card> GenerateCardsByDeck(Deck deck)
        {
            var deckData = DataService.Instance.GetDeckData(deck);
            foreach (var card in deckData.needTypeList)
            {
                for (var i = 0; i < deckData.Get(card); ++i)
                {
                    yield return card.card;
                }
            }
        }

        /// <summary>
        /// 按照输入的职业顺序得到对应的牌组。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IEnumerable<Card>> GetDeckEnumerator()
        {
            foreach (var deck in _decks)
            {
                yield return deck;
            }
        }
    }
}