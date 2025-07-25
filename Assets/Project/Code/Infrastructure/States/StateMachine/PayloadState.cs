using Cysharp.Threading.Tasks;
using System;

namespace Assets.Code.Scripts.Runtime.State_Machine
{
    [Serializable]
    public abstract class PayloadState<TPayload> : State, IPayloadState<TPayload>
    {
        public PayloadState(StateMachine stateMachine)
            : base(stateMachine) { }

        public abstract UniTask OnEnterAsync(TPayload payload);
    }
}