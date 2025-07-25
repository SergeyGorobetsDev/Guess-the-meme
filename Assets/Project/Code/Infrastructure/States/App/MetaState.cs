using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class MetaState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly IAudioPlayer audioPlayer;

        [Inject]
        public MetaState(StateMachine stateMachine,
                         IWindowsNavigator windowsNavigator,
                         IAudioPlayer audioPlayer) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.audioPlayer = audioPlayer;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            windowsNavigator.Show<LoadingWindow>();
            audioPlayer.PlayMusic("music");
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).ToUniTask();
            windowsNavigator.Show<MainMenuWindow>();
        }
    }
}
