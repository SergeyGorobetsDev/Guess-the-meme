using Assets.Code.Scripts.Runtime.AssetManagement;
using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Code.Scripts.Runtime.Progress;
using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Actors.Spawner;
using Assets.Project.Code.Gameplay.Banner;
using Assets.Project.Code.Gameplay.CameraSystem;
using Assets.Project.Code.Gameplay.Level;
using Assets.Project.Code.Gameplay.StaticData;
using Assets.Project.Code.Gameplay.Timer;
using Assets.Project.Code.Gameplay.Zones;
using Assets.Project.Code.Infrastructure.States.App;
using Assets.Project.Code.Progress.ProgressData;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable, ITickable
    {
        [SerializeField]
        private AudioPlayer audioPlayer;

        private StateMachine stateMachine;

        public void Initialize()
        {
            IStatesFactory statesFactory = Container.Resolve<IStatesFactory>();
            stateMachine = Container.Resolve<StateMachine>();
            stateMachine.AddStates(
                statesFactory.Create<BootstrapState>(),
                statesFactory.Create<LoadProgressState>(),
                statesFactory.Create<MetaState>(),
                statesFactory.Create<LoadGameplayState>(),
                statesFactory.Create<GameplayStartState>(),
                statesFactory.Create<GameplayLoopState>(),
                statesFactory.Create<GameplayLooseState>(),
                statesFactory.Create<GameplayWinState>(),
                statesFactory.Create<GameplayRestartState>(),
                statesFactory.Create<AppQuitState>()
            );

            stateMachine.SetState<BootstrapState>();
        }

        public void Tick()
        {
            stateMachine?.Update();
        }

        public override void InstallBindings()
        {
            BindFactory();
            BindServices();
            BindCommonServices();
            BindProviders();
            BindAppStates();
            BindGameplay();

            Container.BindInterfacesTo<BootstrapInstaller>()
                     .FromInstance(this).AsSingle();
        }

        private void BindFactory()
        {
            Container.BindInterfacesAndSelfTo<StatesFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<WindowsFactory>().FromNew().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<ProgressDataService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<AssetProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<StaticDataService>().FromNew().AsSingle();
        }

        private void BindCommonServices()
        {
            Container.BindInterfacesAndSelfTo<WindowsNavigator>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
        }

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<ProgressProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<FileProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnAreaProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<ZoneProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelDataProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<BannerProvider>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraProvider>().FromNew().AsSingle();
        }

        private void BindAppStates()
        {
            Container.BindInterfacesAndSelfTo<StateMachine>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<BootstrapState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadProgressState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<MetaState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadGameplayState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStartState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayLoopState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayLooseState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayWinState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayRestartState>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<AppQuitState>().FromNew().AsSingle();
        }

        private void BindGameplay()
        {
            Container.BindInterfacesAndSelfTo<ActorsSpawner>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<Timer>().FromNew().AsSingle();
        }
    }
}