using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Instruction;
using NUnit.Framework;
using UnityEngine.TestTools;
using GameLib.Common.Extension;

namespace Test.Editor.InstructionTest
{
    [TestFixture]
    public class InstructionTest
    {
        private readonly List<ulong> _idList = new() { 1, 2, 3};
        
        private Context _context;

        [SetUp]
        public void Setup()
        {
            _context = new Context(_idList, _idList);
        }

        [UnityTest]
        public IEnumerator TestEngage()
        {
            yield return InitDB().AsEnumeratorReturnNull();

            var beforeHandNum = GetPlayer1().Hands.Count;
            var beforeDrawNum = GetPlayer2().Draws.Count;
            
            var action = GetAction(Card.Engage);
            yield return action.Execution(_context, _idList.First()).AsEnumeratorReturnNull();

            var afterHandNum = GetPlayer1().Hands.Count;
            var afterDrawNum = GetPlayer2().Draws.Count;
            
            Assert.IsTrue(beforeHandNum == afterHandNum-3);
            Assert.IsTrue(beforeDrawNum == afterDrawNum+3);
        }

        [UnityTest]
        public IEnumerator TestDonation()
        {
            yield return InitDB().AsEnumeratorReturnNull();

            var beforeHandNum1 = GetPlayer1().Hands.Count;
            var beforeHandNum2 = GetPlayer2().Hands.Count;
            
            GetPlayer1().SetInteractiveMode(InteractiveMode.P2);
            var action = GetAction(Card.Donation);
            yield return action.Execution(_context, _idList.First()).AsEnumeratorReturnNull();

            var afterHandNum1 = GetPlayer1().Hands.Count;
            var afterHandNum2 = GetPlayer2().Hands.Count;
            
            Assert.AreEqual(afterHandNum1, 0);
            Assert.AreEqual(afterHandNum2, beforeHandNum1+beforeHandNum2);
        }
        
        [UnityTest]
        public IEnumerator TestHealingHerb()
        {
            yield return InitDB().AsEnumeratorReturnNull();
            
            var beforeHandNum2 = GetPlayer2().Hands.Count;
            
            GetPlayer1().SetInteractiveMode(InteractiveMode.P2);
            var action = GetAction(Card.HealingHerbs);
            yield return action.Execution(_context, _idList.First()).AsEnumeratorReturnNull();

            var afterHandNum2 = GetPlayer2().Hands.Count;
            
            Assert.AreEqual(beforeHandNum2+4, afterHandNum2);
        }
        
        [UnityTest]
        public IEnumerator TestHeal()
        {
            yield return InitDB().AsEnumeratorReturnNull();
            
            var beforeDrawNum1 = GetPlayer1().Draws.Count;
            var beforeDiscardNum1 = GetPlayer1().Discards.Count;
            
            GetPlayer2().SetInteractiveMode(InteractiveMode.P1);
            var action = GetAction(Card.Heal);
            yield return action.Execution(_context, _idList.Last()).AsEnumeratorReturnNull();

            var afterDrawsCount = GetPlayer1().Draws.Count;
            
            Assert.AreEqual(beforeDrawNum1+beforeDiscardNum1, afterDrawsCount);
        }
        
        [UnityTest]
        public IEnumerator TestMagicBomb()
        {
            yield return InitDB().AsEnumeratorReturnNull();
            
            var action = GetAction(Card.MagicBomb);
            yield return action.Execution(_context, _idList.First()).AsEnumeratorReturnNull();

            Assert.AreEqual(5, _context.GetLevel().resources.Keys.Count);
        }

        [UnityTest]
        public IEnumerator TestInspire()
        {
            yield return InitDB().AsEnumeratorReturnNull();
            
            var beforeHandNum1 = GetPlayer1().Hands.Count;
            var beforeDiscardNum1 = GetPlayer1().Discards.Count;
            var beforeHandNum2 = GetPlayer2().Hands.Count;
            
            var action = GetSkill(Skill.Inspire);
            yield return action.Execution(_context, _idList.First()).AsEnumeratorReturnNull();

            var afterHandNum1 = GetPlayer1().Hands.Count;
            var afterDiscardNum1 = GetPlayer1().Discards.Count;
            var afterHandNum2 = GetPlayer2().Hands.Count;
            
            Assert.AreEqual(beforeHandNum2+2, afterHandNum2);
            Assert.AreEqual(beforeHandNum1-3, afterHandNum1);
            Assert.AreEqual(beforeDiscardNum1+3, afterDiscardNum1);
        }
        
        [UnityTest]
        public IEnumerator TestLockedDoor()
        {
            yield return InitDB().AsEnumeratorReturnNull();
            
            var beforeHandsNum1 = GetPlayer1().Hands.Count;
            var beforeHandsNum2 = GetPlayer2().Hands.Count;
            
            var action = GetEvent(Challenge.LockedDoor);
            yield return action.Execution(_context, 0).AsEnumeratorReturnNull();

            var afterHandsNum1 = GetPlayer1().Hands.Count;
            var afterHandsNum2 = GetPlayer2().Hands.Count;
            
            Assert.AreEqual(beforeHandsNum1-3, afterHandsNum1);
            Assert.AreEqual(beforeHandsNum2-3, afterHandsNum2);
        }
        
        [UnityTest]
        public IEnumerator TestYetMoreSpike()
        {
            yield return InitDB().AsEnumeratorReturnNull();

            GetPlayer1().SetInteractiveMode(InteractiveMode.P1);
            GetPlayer2().SetInteractiveMode(InteractiveMode.P2);
            GetPlayer3().SetInteractiveMode(InteractiveMode.P1);
            
            var action = GetEvent(Challenge.YetMoreSpikes);
            yield return action.Execution(_context, 0).AsEnumeratorReturnNull();

            var afterHandsNum = GetPlayer1().Hands.Count;
            
            Assert.AreEqual(0, afterHandsNum);
        }
        
        private async Task InitDB()
        {
            while (!DataService.Instance.IsInitialized())
            {
                await Task.Delay(100);
            }
        }
        
        private InstructionGraph GetAction(Card card)
        {
            var data = DataService.Instance.GetPlayerCardData(card);
            return data.action.Copy() as InstructionGraph;
        }

        private InstructionGraph GetEvent(Challenge challenge)
        {
            var data = DataService.Instance.GetChallengeCardData(challenge);
            return data.action.Copy() as InstructionGraph;
        }

        private InstructionGraph GetSkill(Skill skill)
        {
            var data = DataService.Instance.GetSkillData(skill);
            return data.action.Copy() as InstructionGraph;
        }

        private Player GetPlayer1()
        {
            return _context.GetPlayer(1);
        }
        
        private Player GetPlayer2()
        {
            return _context.GetPlayer(2);
        }
        
        private Player GetPlayer3()
        {
            return _context.GetPlayer(3);
        }
    }
}