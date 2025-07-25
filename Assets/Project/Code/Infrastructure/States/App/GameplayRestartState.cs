using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Actors.Spawner;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class GameplayRestartState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly IInputService inputService;
        private readonly IActorsProvider actorsProvider;
        private readonly IActorsSpawner actorsSpawner;
        private readonly ISpawnAreaProvider spawnAreaProvider;
        private readonly IAudioPlayer audioPlayer;

        [Inject]
        public GameplayRestartState(StateMachine stateMachine,
                                    IWindowsNavigator windowsNavigator,
                                    IInputService inputService,
                                    IActorsProvider actorsProvider,
                                    IActorsSpawner actorsSpawner,
                                    ISpawnAreaProvider spawnAreaProvider,
                                    IAudioPlayer audioPlayer) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.inputService = inputService;
            this.actorsProvider = actorsProvider;
            this.actorsSpawner = actorsSpawner;
            this.spawnAreaProvider = spawnAreaProvider;
            this.audioPlayer = audioPlayer;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnExitAsync();
            inputService.ChangeInputMap(InputMapType.UI);

            foreach (var actor in actorsProvider.Actors)
                actor.gameObject.SetActive(true);
            actorsSpawner.RespawnActiveActors(spawnAreaProvider.GetSpawnArea().SpawnPositions());
            actorsSpawner.RespawnCharacter(spawnAreaProvider.GetSpawnArea().SpawnPositions());
            audioPlayer.PlayMusic("music");
            stateMachine.SetState<GameplayLoopState>();
        }
    }
}