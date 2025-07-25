using Assets.Code.Scripts.Runtime.State_Machine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Code.Gameplay.Actors
{
    public sealed class MoveToZoneState : PayloadState<Vector3>
    {
        private Vector3 position;

        private readonly NavMeshAgent agent;
        private readonly Rigidbody rigidbody;

        public MoveToZoneState(StateMachine stateMachine, NavMeshAgent agent, Rigidbody rigidbody) : base(stateMachine)
        {
            this.agent = agent;
            this.rigidbody = rigidbody;
        }

        public override async UniTask OnEnterAsync(Vector3 position)
        {
            this.position = position;
            this.rigidbody.isKinematic = false;
            this.agent.isStopped = false;
            this.agent.SetDestination(position);
            await UniTask.CompletedTask;
        }

        public override void OnUpdate()
        {
            if (position == Vector3.zero)
                return;

            if (TargetPositionReached())
                stateMachine.SetState<WaitState>();
            else agent.SetDestination(position);
        }

        private bool TargetPositionReached()
        {
            if (!agent.pathPending)
                if (agent.remainingDistance <= agent.stoppingDistance)
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        return true;

            return false;
        }
    }
}