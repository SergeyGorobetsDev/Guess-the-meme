using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Code.Scripts.Runtime.State_Machine
{
    [Serializable]
    public abstract class State
    {
        [field: SerializeField]
        public bool IsActive { get; set; }

        protected StateMachine stateMachine;

        public State() { }

        public State(StateMachine parent) =>
            this.stateMachine = parent;

        public virtual async UniTask OnEnterAsync()
        {
            await UniTask.CompletedTask;
        }

        public virtual void OnUpdate()
        {
        }

        public virtual async UniTask OnExitAsync()
        {
            await UniTask.CompletedTask;
        }
    }
}
