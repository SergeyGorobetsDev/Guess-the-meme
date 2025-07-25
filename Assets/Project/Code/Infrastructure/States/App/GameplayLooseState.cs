using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem;
using Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Infrastructure.States.App
{
    public class GameplayLooseState : State
    {
        private readonly IWindowsNavigator windowsNavigator;
        private readonly IInputService inputService;
        private readonly IAudioPlayer audioPlayer;

        [Inject]
        public GameplayLooseState(StateMachine stateMachine,
                                  IWindowsNavigator windowsNavigator,
                                  IInputService inputService,
                                  IAudioPlayer audioPlayer) : base(stateMachine)
        {
            this.windowsNavigator = windowsNavigator;
            this.inputService = inputService;
            this.audioPlayer = audioPlayer;
        }

        public override async UniTask OnEnterAsync()
        {
            await base.OnExitAsync();
            audioPlayer.StopMusic();
            audioPlayer.Play("loose-game", MixerTarget.SFX);
            inputService.ChangeInputMap(InputMapType.UI);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            windowsNavigator.Show<LooseGameWindow>();
        }
    }
}