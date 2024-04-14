using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Instruction;
using Gameplay.Core.State;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core
{
    /// <summary>
    /// 玩法服务器，主要控制游戏玩法进程。
    /// </summary>
    public class GamePlayService : NetworkBehaviour
    {
        private GameplayServiceState _curState;

        private readonly Dictionary<string, GameplayServiceState> _states = new();

        private void Update()
        {
            _curState?.Update();
        }

        /// <summary>
        /// 启动服务。
        /// </summary>
        public async void StartService()
        {
            if (!IsServer) return;
            await ChangeState<BeginState>();
        }

        public async Task ChangeState<T>() where T : GameplayServiceState, new()
        {
            if (!IsServer) return;
            Debug.Log($"进入{typeof(T).Name}状态。");
            CreateStateIfNeeded<T>();

            if (_curState != null)
            {
                await _curState.Exit();
            }
            _curState = _states[GetKey<T>()];
            await _curState.Enter();
        }

        private string GetKey<T>() where T : GameplayServiceState, new()
        {
            return typeof(T).Name;
        }

        private void CreateStateIfNeeded<T>() where T : GameplayServiceState, new()
        {
            var key = GetKey<T>();
            if (_states.ContainsKey(key)) return;
            _states[key] = new T();
            _states[key].SetService(this);
        }

        /// <summary>
        /// 执行释放技能效果。
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="param"></param>
        [ServerRpc(RequireOwnership = false)]
        public void CastSkillServerRpc(Skill skill, ServerRpcParams param=default)
        {
            _curState.ExecuteAction(
                new GameAction
                {
                    graph = DataService.Instance.GetSkillData(skill).action.Copy() as InstructionGraph,
                    clientID = param.Receive.SenderClientId,
                }
            );
        }

        /// <summary>
        /// 执行打出手牌效果。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="param"></param>
        [ServerRpc(RequireOwnership = false)]
        public void PlayCardServerRpc(Card card, ServerRpcParams param=default)
        {
            _curState.ExecuteAction(
                new GameAction
                {
                    graph = DataService.Instance.GetPlayerCardData(card).action.Copy() as InstructionGraph,
                    clientID = param.Receive.SenderClientId,
                }
            );
        }
    }
}