using Assets.Code.Scripts.Runtime.AssetManagement;
using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Code.Scripts.Runtime.State_Machine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class BootstrapState : State
    {
        private readonly IAssetProvider assetProvider;
        private readonly IInputService inputService;

        [Inject]
        public BootstrapState(StateMachine stateMachine,
                              IAssetProvider assetProvider,
                              IInputService inputService) : base(stateMachine)
        {
            this.assetProvider = assetProvider;
            this.inputService = inputService;
        }

        public override async UniTask OnEnterAsync()
        {

            await assetProvider.InitializeAsync();
            inputService.Initialize();
            stateMachine.SetState<LoadProgressState>();
        }
    }
}