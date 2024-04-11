using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Instruction;
using GameLib.Common.DataStructure;
using UnityEngine;

namespace Test.Editor.InstructionTest
{
    public class Context : ICmdContext
    {
        private readonly Dictionary<ulong, Player> _controllers = new();

        private readonly LevelController _levelController;
        
        public Context(IEnumerable<ulong> idList, IEnumerable<ulong> enemyIDList)
        {
            foreach (var id in idList)
            {
                _controllers[id] = new Player(id, this);
            }

            _levelController = new(enemyIDList);
        }

        public Player GetPlayer(ulong clientID)
        {
            return _controllers[clientID];
        }
        
        public IPlayerController GetPlayerController(ulong clientID)
        {
            return _controllers[clientID];
        }

        public IEnumerable<ulong> GetAllClientIDs()
        {
            return _controllers.Keys;
        }

        public ILevelController GetLevelController()
        {
            return _levelController;
        }

        public ITimeController GetTimeController()
        {
            throw new NotImplementedException();
        }

        public LevelController GetLevel()
        {
            return _levelController;
        }
    }
    
    public class Player : IPlayerController
    {
        public readonly Stack<Card> Draws = new ();
        public readonly List<Card> Hands= new ();
        public readonly Stack<Card> Discards= new ();

        private Context _context;
        private IPlayerInteractive _interactive;
        private IPlayerInteractive _interactive1;
        private IPlayerInteractive _interactive2;
        private IPlayerInteractive _interactiveAll;
        
        public Player(ulong clientID, Context context)
        {
            ClientID = clientID;
            _context = context;
            _interactive1 = new PlayerInteractive(this, 1);
            _interactive2 = new PlayerInteractive(this, 2);
            _interactiveAll = new PlayerInteractive(this, 1, 2, 3);
            SetInteractiveMode(InteractiveMode.All);
            InitPile();
        }

        private void InitPile()
        {
            var cards = new List<Card>()
            {
                Card.Arrow, Card.Sword, Card.SwordArrow, Card.Cancel, Card.Heal, Card.HealingHerbs, Card.Steal
            };
            foreach (var card in cards)
            {
                Draws.Push(card);
            }

            var discards = new List<Card>()
            {
                Card.MagicBomb, Card.DivineShield, Card.Cancel, Card.Fireball, Card.MagicBomb,
            };
            foreach (var card in discards)
            {
                Discards.Push(card);
            }
            
            Hands.Add(Card.TwoArrow);
            Hands.Add(Card.Shield);
            Hands.Add(Card.SwordArrow);
            Hands.Add(Card.Arrow);
        }
        
        public ulong ClientID { get; }
        
        public void InitDeck(IEnumerable<Card> cards)
        {
        }

        public void Draw(int num)
        {
            var realNum = Math.Min(num, Draws.Count);
            for (var i = 0; i < realNum; ++i)
            {
                Hands.Add(Draws.Pop());
            }
        }

        public void Discard(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                Hands.Remove(card);
                Discards.Push(card);
            }
        }

        public void DrawFromDiscard(int num)
        {
            var realNum = Math.Min(num, Discards.Count);
            for (var i = 0; i < realNum; ++i)
            {
                Hands.Add(Discards.Pop());
            }
        }

        public void DiscardToDraw(int num)
        {
            var realNum = Math.Min(num, Discards.Count);
            for (var i = 0; i < realNum; ++i)
            {
                Draws.Push(Discards.Pop());
            }
        }

        public void DiscardResource(Resource type)
        {
            var discardCards = from card in Hands
                where DataHelper.IsCardContainResource(card, type)
                select card;
            Discard(discardCards.ToList());
        }

        public void GiveHand(ulong targetPlayerID)
        {
            var controller = _context.GetPlayerController(targetPlayerID);
            controller.AddHand(Hands);
            Hands.Clear();
        }

        public void AddHand(IEnumerable<Card> cardList)
        {
            Hands.AddRange(cardList);
        }

        public void SetInteractiveMode(InteractiveMode mode)
        {
            _interactive = mode switch
            {
                InteractiveMode.P1 => _interactive1,
                InteractiveMode.P2 => _interactive2,
                InteractiveMode.All => _interactiveAll,
                _ => _interactive1,
            };

        }

        public IPlayerInteractive GetInteractiveHandler()
        {
            return _interactive;
        }
    }
    
    public class LevelController : ILevelController
    {
        public readonly List<ulong> enemies = new();
        
        public readonly Counter<Resource> resources = new ();
        
        public LevelController(IEnumerable<ulong> idList)
        {
            enemies.AddRange(idList);
        }
        public void AddResource(Resource type, int num = 1)
        {
            resources[type] += 1;
        }

        public void DestroyEnemyCard(ulong enemyID)
        {
            enemies.Remove(enemyID);
        }

        public void RevealNextLevel(int num)
        {
            Debug.Log($"reveal {num}");
        }

        public void StopTime()
        {
            Debug.Log("Stop time");
        }

        public void ContinueTime()
        {
            Debug.Log("Continue time");
        }

        public IEnumerable<ulong> GetEnemyIDs()
        {
            return enemies;
        }
    }

    public class PlayerInteractive : IPlayerInteractive
    {
        private readonly Player _player;

        private readonly List<ulong> _selectList;

        public PlayerInteractive(Player player, params ulong[] vars)
        {
            _player = player;
            _selectList = new List<ulong>(vars);
        }
        
        public async Task<List<ulong>> SelectPlayers(int num, bool canSelectSelf)
        {
            await Task.Delay(1000);
            return _selectList;
        }

        public async Task<List<Card>> SelectHandCards(int num)
        {
            await Task.Delay(1000);
            return _player.Hands.GetRange(0, Math.Min(num, _player.Hands.Count));
        }

        public async Task<ulong> SelectEnemy()
        {
            await Task.Delay(1000);
            return 1;
        }

        public async Task<Resource> SelectResource()
        {
            await Task.Delay(1000);
            return Resource.Arrow;
        }
    }

    public enum InteractiveMode
    {
        P1,
        P2,
        All,
    }
}