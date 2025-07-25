using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Infrastructure.States.App;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows
{
    public sealed class WinGameWindow : Window
    {
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button exitButton;

        private StateMachine stateMachine;

        [Inject]
        public void Constructor(StateMachine stateMachine, IWindowsNavigator windowsNavigator)
        {
            this.stateMachine = stateMachine;
            this.windowsNavigator = windowsNavigator;
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            restartButton.onClick.AddListener(() =>
            {
                stateMachine.SetState<GameplayRestartState>();
            });

            exitButton.onClick.AddListener(() =>
            {
                stateMachine.SetState<MetaState>();
            });
        }

        protected override void UnRegisterCallbacks()
        {
            base.UnRegisterCallbacks();
            restartButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}