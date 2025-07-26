using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Actors.Spawner;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class GameplayEndState : State
    {
        private readonly IActorsSpawner actorsSpawner;

        [Inject]
        public GameplayEndState(StateMachine stateMachine,
                                IActorsSpawner actorsSpawner) : base(stateMachine)
        {
            this.actorsSpawner = actorsSpawner;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            actorsSpawner.Clear();
            stateMachine.SetState<MetaState>();
        }
    }
}