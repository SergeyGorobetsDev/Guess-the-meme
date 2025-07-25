using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Actors.Spawner;
using Assets.Project.Code.Gameplay.Banner;
using Assets.Project.Code.Gameplay.Level;
using Assets.Project.Code.Gameplay.StaticData;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class GameplayStartState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly IStaticDataService staticDataService;
        private readonly IActorsSpawner actorsSpawner;
        private readonly ISpawnAreaProvider spawnAreaProvider;
        private readonly ILevelDataProvider levelDataProvider;
        private readonly IBannerProvider bannerProvider;

        [Inject]
        public GameplayStartState(StateMachine stateMachine,
                                  IWindowsNavigator windowsNavigator,
                                  IStaticDataService staticDataService,
                                  IActorsSpawner actorsSpawner,
                                  ISpawnAreaProvider spawnAreaProvider,
                                  ILevelDataProvider levelDataProvider,
                                  IBannerProvider bannerProvider) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.staticDataService = staticDataService;
            this.actorsSpawner = actorsSpawner;
            this.spawnAreaProvider = spawnAreaProvider;
            this.levelDataProvider = levelDataProvider;
            this.bannerProvider = bannerProvider;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            List<UnityEngine.Vector3> positions = spawnAreaProvider.GetSpawnArea().SpawnPositions(staticDataService.Actor.Radius);
            actorsSpawner.SpawnCharacter(positions);
            actorsSpawner.SpawnActors(levelDataProvider.GetActorAmountToSpawn(), positions);
            stateMachine.SetState<GameplayLoopState>();
        }
    }
}
