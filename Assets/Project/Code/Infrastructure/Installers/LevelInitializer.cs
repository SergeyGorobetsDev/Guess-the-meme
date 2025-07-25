using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Actors.Spawner;
using Assets.Project.Code.Gameplay.Banner;
using Assets.Project.Code.Gameplay.CameraSystem;
using Assets.Project.Code.Gameplay.Zones;
using Assets.Project.Code.Infrastructure.States.App;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Infrastructure.Installers
{
    public class LevelInitializer : MonoBehaviour, IInitializable
    {
        [Header("Level Components")]
        [SerializeField]
        private ZoneTrigger[] zoneTriggers;

        [SerializeField]
        private SpawnArea spawnArea;

        [SerializeField]
        private BannerHandler bannerHandler;

        [SerializeField]
        private Camera mainCam;

        [SerializeField]
        private CameraHandler cameraHandler;

        private IZoneProvider zoneProvider;
        private ISpawnAreaProvider spawnAreaProvider;
        private IBannerProvider bannerProvider;
        private ICameraProvider cameraProvider;
        private StateMachine stateMachine;

        [Inject]
        public void Constructor(IZoneProvider zoneProvider,
                                ISpawnAreaProvider spawnAreaProvider,
                                IBannerProvider bannerProvider,
                                ICameraProvider cameraProvider,
                                StateMachine stateMachine)
        {
            this.zoneProvider = zoneProvider;
            this.spawnAreaProvider = spawnAreaProvider;
            this.bannerProvider = bannerProvider;
            this.cameraProvider = cameraProvider;
            this.stateMachine = stateMachine;
        }

        public void Initialize()
        {
            zoneProvider.Initialize(zoneTriggers);
            spawnAreaProvider.Initialize(spawnArea);
            bannerProvider.Initialize(bannerHandler);
            cameraProvider.Initialize(cameraHandler, mainCam);
            stateMachine.SetState<GameplayStartState>();
        }
    }
}
