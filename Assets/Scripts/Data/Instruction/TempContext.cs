using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Data.Instruction
{
    /// <summary>
    /// 临时上下文环境。
    /// </summary>
    public class TempContext
    {
        public TempContext(ulong subjectID, ulong clientID, TempContextGroup group=null)
        {
            SubjectID = subjectID;
            ClientID = clientID;
            Group = group;
        }

        /// <summary>
        /// 上下文所在的组。
        /// </summary>
        public TempContextGroup Group { get; }

        /// <summary>
        /// 玩家ID。
        /// </summary>
        public ulong ClientID { private set; get; }
        
        /// <summary>
        /// 行动的发起者ID
        /// </summary>
        public ulong SubjectID { private set; get; }

        public override string ToString()
        {
            return $"SubjectID={SubjectID}";
        }
    }

    /// <summary>
    /// 临时环境组。
    /// </summary>
    public class TempContextGroup : IEnumerable<TempContext>
    {
        private readonly List<TempContext> _contexts = new();

        private readonly DecisionMaker<Resource> _resourceDecision;

        private readonly DecisionMaker<ulong> _playerDecision;

        public bool IsMakeDecision { private set; get; }
        
        public TempContextGroup(ulong subjectID, IEnumerable<ulong> clientsIDList)
        {
            var playerList = clientsIDList.ToList();
            _resourceDecision = new DecisionMaker<Resource>(playerList.Count, OnChoiceResource);
            _playerDecision = new DecisionMaker<ulong>(playerList.Count(), OnChoicePlayer);
            foreach (var clientID in playerList)
            {
                _contexts.Add(new TempContext(subjectID, clientID, this));
            }
        }

        /// <summary>
        /// 添加玩家选择。
        /// </summary>
        /// <param name="choice"></param>
        public void AddChoice(ulong choice)
        {
            _playerDecision.AddChoice(choice);
        }

        /// <summary>
        /// 添加资源选择。
        /// </summary>
        /// <param name="choice"></param>
        public void AddChoice(Resource choice)
        {
            _resourceDecision.AddChoice(choice);
        }

        /// <summary>
        /// 获得被选择最多的玩家。
        /// </summary>
        /// <returns></returns>
        public ulong GetMostPlayer()
        {
            return _playerDecision.GetTheMostChoice();
        }

        /// <summary>
        /// 获得被选择最多的资源。
        /// </summary>
        /// <returns></returns>
        public Resource GetMostResource()
        {
            return _resourceDecision.GetTheMostChoice();
        }

        public IEnumerator<TempContext> GetEnumerator()
        {
            return _contexts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnChoiceResource(Resource type)
        {
            IsMakeDecision = true;
        }

        private void OnChoicePlayer(ulong clientID)
        {
            IsMakeDecision = true;
        }
    }
}