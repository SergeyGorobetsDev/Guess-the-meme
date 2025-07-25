using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Interactable;
using Assets.Project.Code.Gameplay.Zones;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.Project.Code.Gameplay.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Actor : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private ActorData actorData;

        [Header("Components")]
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private Rigidbody rigBody;
        [SerializeField]
        private CapsuleCollider capsuleCollider;

        [field: SerializeField]
        public ZoneCorrectness ZoneCorrectnessm { get; private set; } = ZoneCorrectness.None;

        private StateMachine stateMachine;

        public float Radius => actorData.Radius;

        private IZoneProvider zoneProvider;

        [Inject]
        public void Constructor(IZoneProvider zoneProvider)
        {
            this.zoneProvider = zoneProvider;
        }


        private void Update()
        {
            stateMachine?.Update();
        }

        public void Initialize()
        {
            stateMachine = new StateMachine();
            stateMachine.AddStates(
                new WaitState(stateMachine, agent, rigBody),
                new DecideState(stateMachine, agent, zoneProvider),
                new MoveToZoneState(stateMachine, agent, rigBody)
            );

            stateMachine.SetState<WaitState>();
        }

        public void MakeChoise()
        {
            stateMachine?.SetState<DecideState>();
        }

        public void Interact(ZoneCorrectness zoneCorrectnessm, bool isInZone)
        {
            ZoneCorrectnessm = zoneCorrectnessm;
            stateMachine.SetState<WaitState>();
        }

        public void Reset()
        {
            ZoneCorrectnessm = ZoneCorrectness.None;
        }
    }
}
