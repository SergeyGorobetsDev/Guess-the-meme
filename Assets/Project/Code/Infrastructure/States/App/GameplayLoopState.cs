using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Actors.Spawner;
using Assets.Project.Code.Gameplay.Banner;
using Assets.Project.Code.Gameplay.Level;
using Assets.Project.Code.Gameplay.StaticData;
using Assets.Project.Code.Gameplay.Timer;
using Assets.Project.Code.Gameplay.Zones;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class GameplayLoopState : State
    {
        private float roundWaitTime = 0;
        private List<int> usedMemes = new(5);
        private readonly IWindowsNavigator windowsNavigator;
        private readonly ILevelDataProvider levelDataProvider;
        private readonly IBannerProvider bannerProvider;
        private readonly IZoneProvider zoneProvider;
        private readonly IActorsProvider actorsProvider;
        private readonly IActorsSpawner actorsSpawner;
        private readonly ISpawnAreaProvider spawnAreaProvider;
        private readonly IStaticDataService staticDataService;
        private readonly IInputService inputService;
        private Timer timer;

        [Inject]
        public GameplayLoopState(StateMachine stateMachine,
                                 IWindowsNavigator windowsNavigator,
                                 ILevelDataProvider levelDataProvider,
                                 IBannerProvider bannerProvider,
                                 IZoneProvider zoneProvider,
                                 IActorsProvider actorsProvider,
                                 IActorsSpawner actorsSpawner,
                                 ISpawnAreaProvider spawnAreaProvider,
                                 IStaticDataService staticDataService,
                                 IInputService inputService,
                                 Timer timer) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.levelDataProvider = levelDataProvider;
            this.bannerProvider = bannerProvider;
            this.zoneProvider = zoneProvider;
            this.actorsProvider = actorsProvider;
            this.actorsSpawner = actorsSpawner;
            this.spawnAreaProvider = spawnAreaProvider;
            this.staticDataService = staticDataService;
            this.inputService = inputService;
            this.timer = timer;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            windowsNavigator.Show<GameplayWindow>();
            roundWaitTime = levelDataProvider.GetRoundTime();
            inputService.ChangeInputMap(InputMapType.Gameplay);
            timer.OnTimerComplete += TimerFinished;
            SetupMeme();
        }

        public override async UniTask OnExitAsync()
        {
            await base.OnExitAsync();
            timer.OnTimerComplete -= TimerFinished;
            timer.StopTimer();
            usedMemes.Clear();

            actorsSpawner.DespawnActors();
            actorsSpawner.DespawnPlayer();
        }

        private void TimerFinished()
        {
            CheckPlayerPlaceZone();
            CheckActorsPlacedZones();
            CheckAllMemesGuessed();
            actorsSpawner.RespawnActiveActors(spawnAreaProvider.GetSpawnArea().SpawnPositions());
            actorsSpawner.RespawnCharacter(spawnAreaProvider.GetSpawnArea().SpawnPositions());
            SetupMeme();
        }

        public override void OnUpdate()
        {
            if (timer.IsRunning)
                timer.Tick();
        }

        private void SetupMeme()
        {
            int memeId = Random.Range(0, staticDataService.MemeImages.Count);

            if (!usedMemes.Contains(memeId))
            {
                usedMemes.Add(memeId);
                SetBannerMeme(memeId);
                SetUpZones(memeId);
                timer.StartTimer(roundWaitTime);
                MakeAIChoices();
            }
            else SetupMeme();
        }

        private void SetBannerMeme(int id)
        {
            bannerProvider.GetBannerHandler().ChangeImage(staticDataService.MemeImages[id]);
        }

        //Не самое лучшее решение, но для прототипа сойдет
        private void SetUpZones(int id)
        {
            ZoneTrigger[] zones = zoneProvider.GetZoneTriggers();

            int rand = Random.Range(0, 2);
            Debug.Log($"Setting up zones with meme ID: {id}, Random value: {rand}");
            zones[0].SetZoneCorrectness(rand == 0 ? ZoneCorrectness.Correct : ZoneCorrectness.Incorrect,
                                        rand == 0 ? staticDataService.MemeImages[id] : staticDataService.MemeImages[GetRandomIndexExcluding(id, staticDataService.MemeImages.Count)]);
            zones[1].SetZoneCorrectness(rand == 1 ? ZoneCorrectness.Correct : ZoneCorrectness.Incorrect,
                                        rand == 1 ? staticDataService.MemeImages[id] : staticDataService.MemeImages[GetRandomIndexExcluding(id, staticDataService.MemeImages.Count)]);
        }

        private void MakeAIChoices()
        {
            foreach (var actor in actorsProvider.Actors)
            {
                if (!actor.gameObject.activeSelf) continue;
                actor.MakeChoise();
            }
        }

        private void CheckAllMemesGuessed()
        {
            if (usedMemes.Count == staticDataService.MemeImages.Count)
                stateMachine.SetState<GameplayWinState>();
        }

        private void CheckPlayerPlaceZone()
        {
            if (!actorsProvider.Player.IsInZone || actorsProvider.Player.ZoneCorrectnessm == ZoneCorrectness.Incorrect)
                stateMachine.SetState<GameplayLooseState>();
        }

        private void CheckActorsPlacedZones()
        {
            foreach (var actor in actorsProvider.Actors)
                if (actor.ZoneCorrectnessm == ZoneCorrectness.Incorrect)
                    actor.gameObject.SetActive(false);

            if (actorsProvider.Actors.All(a => !a.gameObject.activeSelf))
                stateMachine.SetState<GameplayWinState>();
        }

        private int GetRandomIndexExcluding(int excludeIndex, int count)
        {
            if (count <= 1)
                return 0;

            int randomIndex = Random.Range(0, count - 1);
            if (randomIndex >= excludeIndex)
                randomIndex++;

            return randomIndex;
        }
    }
}