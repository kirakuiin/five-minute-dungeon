﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Instruction;
using GameLib.Network.NGO;
using Gameplay.Core.State;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Core
{
    /// <summary>
    /// 玩法服务器，主要控制游戏玩法进程。
    /// </summary>
    public class GamePlayService : NetworkSingleton<GamePlayService>
    {
        private GameplayServiceState _curState;

        private readonly Dictionary<string, GameplayServiceState> _states = new();

        private GamePlayContext Context => GamePlayContext.Instance;

        private void Update()
        {
            if (!IsServer) return;
            _curState?.Update();
        }

        /// <summary>
        /// 启动服务。
        /// </summary>
        public async void StartService()
        {
            if (!IsServer) return;
            InitListen();
            await ChangeState<BeginState>();
        }

        private void InitListen()
        {
            foreach (var clientID in Context.GetAllClientIDs())
            {
                var hand = Context.GetPlayerRuntimeInfo(clientID).GetHands();
                hand.OnCardChanged += @event => OnCardChange(clientID);
            }
        }
        
        private void OnCardChange(ulong clientID)
        {
            Debug.Log(clientID);
            var handSize = Context.GetPlayerRuntimeInfo(clientID).GetHands().Count;
            var diff = Context.InitHandNum - handSize;
            if (diff > 0)
            {
                var controller = Context.GetPlayerController(clientID);
                controller.Draw(diff);
            }
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
        public void CastSkill(Skill skill)
        {
            CastSkillServerRpc(skill);
        }

        [Rpc(SendTo.Server)]
        private void CastSkillServerRpc(Skill skill, RpcParams param=default)
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
        public void PlayCard(Card card)
        {
            PlayCardServerRpc(card);
        }

        [Rpc(SendTo.Server)]
        private void PlayCardServerRpc(Card card, RpcParams param=default)
        {
            var controller = Context.GetPlayerController(param.Receive.SenderClientId);
            controller.Play(card);
            _curState.ExecuteAction(
                new GameAction
                {
                    graph = DataService.Instance.GetPlayerCardData(card).action.Copy() as InstructionGraph,
                    clientID = param.Receive.SenderClientId,
                }
            );
        }

        /// <summary>
        /// 丢弃手牌。
        /// </summary>
        /// <param name="card"></param>
        public void DiscardCard(Card card)
        {
            DiscardCardServerRpc(card);
        }

        [Rpc(SendTo.Server)]
        private void DiscardCardServerRpc(Card card, RpcParams param=default)
        {
            var controller = Context.GetPlayerController(param.Receive.SenderClientId);
            controller.Discard(new []{card});
        }
    }
}