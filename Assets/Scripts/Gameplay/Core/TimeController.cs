using System;
using Data.Instruction;
using Unity.Netcode;

namespace Gameplay.Core
{
    public class TimeController : NetworkBehaviour, ITimeController, ITimeRuntimeInfo
    {
        public void StartTimer(int totalCountdown)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }

        public int RemainTime { get; }
        
        public event Action<int> OnTimeUpdated;
        
        public event Action OnTimeout;
    }
}