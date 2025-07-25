using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Gameplay.Timer;
using Assets.Project.Code.Gameplay.Timer.UI;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows
{
    public class GameplayWindow : Window
    {

        //[SerializeField]
        //private CurrencyView currencyView;

        [SerializeField]
        private TimerView timerView;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button pauseButton;

        private StateMachine stateMachine;
        private Timer timer;
        private IAudioPlayer audioPlayer;

        [Inject]
        public void Constructor(StateMachine stateMachine, IWindowsNavigator windowsNavigator, Timer timer, IAudioPlayer audioPlayer)
        {
            this.stateMachine = stateMachine;
            this.windowsNavigator = windowsNavigator;
            this.timer = timer;
            this.audioPlayer = audioPlayer;
        }

        public override void Show()
        {
            base.Show();
        }

        protected override void BindDocumentData()
        {
            base.BindDocumentData();
            timerView.Initialize(timer);
            //currencyView.Initalize(LevelManager.Instance.CurrencyProvider);
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            settingsButton.onClick.AddListener(() =>
            {
                audioPlayer.Play("button-click", MixerTarget.UI);
                windowsNavigator.Show<SettingsWindow>();
            });

            pauseButton.onClick.AddListener(() =>
            {
                audioPlayer.Play("button-click", MixerTarget.UI);
                windowsNavigator.Show<PauseMenu>();
            });
        }

        protected override void UnRegisterCallbacks()
        {
            settingsButton.onClick.RemoveAllListeners();
            pauseButton.onClick.RemoveAllListeners();
            base.UnRegisterCallbacks();
        }
    }
}