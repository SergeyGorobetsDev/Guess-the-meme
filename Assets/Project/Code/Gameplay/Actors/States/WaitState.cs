using Assets.Code.Scripts.Runtime.State_Machine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Code.Gameplay.Actors
{
    public sealed class WaitState : State
    {
        private readonly NavMeshAgent agent;
        private readonly Rigidbody rigidbody;

        public WaitState(StateMachine stateMachine, NavMeshAgent agent, Rigidbody rigidbody) : base(stateMachine)
        {
            this.agent = agent;
            this.rigidbody = rigidbody;
        }

        public override async UniTask OnEnterAsync()
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            rigidbody.isKinematic = true;
            await UniTask.CompletedTask;
        }
    }
}