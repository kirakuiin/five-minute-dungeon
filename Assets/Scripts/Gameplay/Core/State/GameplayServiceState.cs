using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Data.Instruction;
using GameLib.Common.Extension;

namespace Gameplay.Core.State
{
    /// <summary>
    /// 玩法状态的抽象类。
    /// </summary>
    public abstract class GameplayServiceState
    {
        protected GamePlayContext Context => GamePlayContext.Instance;
        
        private GamePlayService Service { set; get; }

        public void SetService(GamePlayService service)
        {
            Service = service;
        }
        
        protected async Task ChangeState<T>() where T : GameplayServiceState, new()
        {
            await Service.ChangeState<T>();
        }
        
        public virtual Task Enter()
        {
            return Task.CompletedTask;
        }

        public virtual Task Exit()
        {  
            return Task.CompletedTask;
        }

        public virtual void ExecuteAction(GameAction action)
        {
        }

        public virtual void Update()
        {
        }
    }
    
    /// <summary>
    /// 一个等待被执行的行动。
    /// </summary>
    public class GameAction
    {
        public InstructionGraph graph;
        
        public ulong clientID;
    }

    /// <summary>
    /// 事件专属行动。
    /// </summary>
    public class EventAction : GameAction
    {
        /// <summary>
        /// 供事件专用的ID
        /// </summary>
        public ulong enemyID;
    }
    
    /// <summary>
    /// 可以执行行动的类。
    /// </summary>
    public abstract class ActionState : GameplayServiceState
    {
        private readonly ConcurrentQueue<GameAction> _actionQueue = new();
        
        private readonly ConcurrentQueue<GameAction> _runningQueue = new();

        private bool _isRunning;

        private const int ExecuteInterval = 50;

        protected event Action<GameAction> OnActionDone;

        protected void StartActionCycle()
        {
            _isRunning = true;
            Executing();
        }

        protected async Task StopActionCycle()
        {
            _isRunning = false;
            await TaskExtension.Wait(() => _actionQueue.IsEmpty && _runningQueue.IsEmpty);
        }

        private async void Executing()
        {
            while (_isRunning || !_actionQueue.IsEmpty || !_runningQueue.IsEmpty)
            {
                if (_actionQueue.TryDequeue(out var action))
                {
                    _runningQueue.Enqueue(action);
                    await action.graph.Execution(Context, action.clientID);
                    OnActionDone?.Invoke(action);
                    _runningQueue.TryDequeue(out var _);
                }
                else
                {
                    await Task.Delay(ExecuteInterval);
                }
            }
        }

        /// <summary>
        /// 执行动作。
        /// </summary>
        /// <param name="action"></param>
        public override void ExecuteAction(GameAction action)
        {
            if (!_isRunning) return;
            _actionQueue.Enqueue(action);
        }
    }
}