using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Zones;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Code.Gameplay.Actors
{
    public sealed class DecideState : State
    {
        private readonly NavMeshAgent agent;
        private readonly IZoneProvider zoneProvider;

        public DecideState(StateMachine stateMachine, NavMeshAgent agent, IZoneProvider zoneProvider) : base(stateMachine)
        {
            this.agent = agent;
            this.zoneProvider = zoneProvider;
        }

        public override async UniTask OnEnterAsync()
        {
            ZoneTrigger[] zones = zoneProvider.GetZoneTriggers();
            Vector3 targetPosition = zones[Random.Range(0, zones.Length)].transform.position;
            stateMachine.SetState<MoveToZoneState, Vector3>(targetPosition);
            await UniTask.CompletedTask;
        }
    }
}