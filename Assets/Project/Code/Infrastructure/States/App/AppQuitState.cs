using Assets.Code.Scripts.Runtime.State_Machine;
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

        [Inject]
        public AppQuitState(StateMachine stateMachine, IWindowsNavigator windowsNavigator) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnEnterAsync();
            windowsNavigator.Show<LoadingWindow>();

#if UNITY_EDITOR
            if (Application.isEditor)
                UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}
