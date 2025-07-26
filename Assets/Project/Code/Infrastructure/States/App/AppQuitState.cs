using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Progress.ProgressData;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class AppQuitState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly ISaveLoadService saveLoadService;

        [Inject]
        public AppQuitState(StateMachine stateMachine, IWindowsNavigator windowsNavigator, ISaveLoadService saveLoadService) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.saveLoadService = saveLoadService;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            windowsNavigator.Show<LoadingWindow>();
            saveLoadService.ActiveData.Version = Application.version;
            await saveLoadService.SaveAsync();
#if UNITY_EDITOR
            if (Application.isEditor)
                UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}
