using Assets.Code.Scripts.Runtime.AssetManagement;
using Assets.Code.Scripts.Runtime.Progress;
using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.StaticData;
using Assets.Project.Code.Progress.ProgressData;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class LoadProgressState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly ISaveLoadService saveLoadService;
        private readonly IStaticDataService staticDataService;
        private readonly IProgressProvider progressProvider;
        private readonly IAudioPlayer audioPlayer;

        [Inject]
        public LoadProgressState(StateMachine stateMachine,
                                 IWindowsNavigator windowsNavigator,
                                 ISaveLoadService saveLoadService,
                                 IStaticDataService staticDataService,
                                 IProgressProvider progressProvider,
                                 IAudioPlayer audioPlayer) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.saveLoadService = saveLoadService;
            this.staticDataService = staticDataService;
            this.progressProvider = progressProvider;
            this.audioPlayer = audioPlayer;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            await staticDataService.LoadAllAssets();
            await windowsNavigator.InitializeAsync(AssetsAddress.WindowSplashScreenAddress,
                                                   AssetsAddress.WindowMainMenuAddress,
                                                   AssetsAddress.WindowSettingsAddress,
                                                   AssetsAddress.WindowPauseMenuAddress,
                                                   AssetsAddress.WindowGameplayAddress,
                                                   AssetsAddress.WindowLooseGameAddress,
                                                   AssetsAddress.WindowWinGameAddress);
            audioPlayer.Initialization();
            windowsNavigator.Show<LoadingWindow>();
            await saveLoadService.LoadAsync();
            progressProvider.SetUserData(saveLoadService.ActiveData);
            stateMachine.SetState<MetaState>();
        }
    }
}
