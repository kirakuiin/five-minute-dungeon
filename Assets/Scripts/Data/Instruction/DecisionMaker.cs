using System;
using UnityEngine.Assertions;
using System.Linq;
using GameLib.Common.DataStructure;

namespace Data.Instruction
{
    /// <summary>
    /// 决断器。
    /// </summary>
    public class DecisionMaker<T>
    {
        private readonly int _playerNum;

        private int _curVoteNum = 0;

        private readonly Action<T> _callback;

        private readonly Counter<T> _counter = new();

        private T _mostChoice;

        private bool _isDone = false;
        
        public DecisionMaker(int num, Action<T> resultCallback)
        {
            Assert.IsTrue(num > 0, "玩家人数必须大于0");
            _playerNum = num;
            _callback = resultCallback;
        }

        /// <summary>
        /// 添加一个新的选择。
        /// </summary>
        /// <param name="choice"></param>
        public void AddChoice(T choice)
        {
            if (_curVoteNum < _playerNum)
            {
                _counter[choice] += 1;
                _curVoteNum += 1;
            }
            if (_curVoteNum == _playerNum)
            {
                _callback?.Invoke(GetTheMostChoice());
            }
        }
        
        /// <summary>
        /// 获得最多的选择。
        /// </summary>
        /// <returns></returns>
        public T GetTheMostChoice()
        {
            if (_isDone) return _mostChoice;
            
            _mostChoice = _counter.MostCommon(1).First().Key;
            _isDone = true;
            
            return _mostChoice;
        }
    }
}