using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Level;
using Assets.Project.Code.Gameplay.StaticData;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class LoadGameplayState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly ILevelDataProvider levelDataProvider;
        private readonly IStaticDataService staticDataService;
        private readonly IAudioPlayer audioPlayer;

        [Inject]
        public LoadGameplayState(StateMachine stateMachine,
                                 IWindowsNavigator windowsNavigator,
                                 ILevelDataProvider levelDataProvider,
                                 IStaticDataService staticDataService,
                                 IAudioPlayer audioPlayer) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.levelDataProvider = levelDataProvider;
            this.staticDataService = staticDataService;
            this.audioPlayer = audioPlayer;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            levelDataProvider.Initialize(staticDataService.LevelData);
            windowsNavigator.Show<LoadingWindow>();
            audioPlayer.PlayMusic("music");
            await SceneManager.LoadSceneAsync(2, LoadSceneMode.Single).ToUniTask();
        }
    }
}
