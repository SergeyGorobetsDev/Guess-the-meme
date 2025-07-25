using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Code.Scripts.Runtime.InputSystem
{
    public sealed class InputService : IInputService, IDisposable, IPlayerInputReader, IUIInputReader
    {
        private readonly InputSystemActions inputActions;
        private readonly InputMapType startUpInputMap = InputMapType.UI;

        [Header("Input Actions")]
        private readonly InputSystemActions.PlayerActions player;
        private readonly InputSystemActions.UIActions ui;

        #region Player Events
        public event Action<Vector2> MovePerformed;
        public event Action<Vector2> MoveCanceled;
        public event Action<Vector2> LookPerformed;
        public event Action<Vector2> LookCanceled;
        #endregion

        public InputAction MoveAction => player.Move;

        #region UI Events
        public event Action<Vector2> NavigatePerformed;
        public event Action SubmitPerformed;
        public event Action CancelPerformed;
        #endregion

        [Inject]
        public InputService()
        {
            this.inputActions = new InputSystemActions();
            this.player = inputActions.Player;
            this.ui = inputActions.UI;
        }

        public void Initialize()
        {
            ChangeInputMap(startUpInputMap);
        }

        public void Dispose()
        {
            player.Disable();
            ui.Disable();
            UnRegisterPlayerEvents();
            UnRegisterUIEvents();
        }

        public void ChangeInputMap(InputMapType inputMapType)
        {
            switch (inputMapType)
            {
                case InputMapType.Gameplay:
                    player.Enable();
                    RegisterPlayerEvents();
                    ui.Disable();
                    break;
                case InputMapType.UI:
                    ui.Enable();
                    RegisterUIEvents();
                    player.Disable();
                    break;
            }
        }

        #region Player Events
        private void RegisterPlayerEvents()
        {
            player.Move.performed += OnMovePerformed;
            player.Move.canceled += OnMovePerformed;
            player.Look.performed += OnLookPerformed;
            player.Look.canceled += OnLookCanceled;
        }

        private void UnRegisterPlayerEvents()
        {
            player.Move.performed -= OnMovePerformed;
            player.Move.canceled -= OnMovePerformed;
            player.Look.performed -= OnLookPerformed;
            player.Look.canceled -= OnLookCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj) =>
            MovePerformed?.Invoke(obj.ReadValue<Vector2>());
        private void OnMoveCanceled(InputAction.CallbackContext obj) =>
           MoveCanceled?.Invoke(obj.ReadValue<Vector2>());

        private void OnLookPerformed(InputAction.CallbackContext obj) =>
            LookPerformed?.Invoke(obj.ReadValue<Vector2>());
        private void OnLookCanceled(InputAction.CallbackContext obj) =>
           LookCanceled?.Invoke(obj.ReadValue<Vector2>());
        #endregion

        #region UI Events
        private void RegisterUIEvents()
        {
            ui.Navigate.performed += OnNavigatePerformed;
            ui.Submit.performed += OnSubmitPerformed;
            ui.Cancel.performed += OnCancelPerformed;
        }

        private void UnRegisterUIEvents()
        {
            ui.Navigate.performed -= OnNavigatePerformed;
            ui.Submit.performed -= OnSubmitPerformed;
            ui.Cancel.performed -= OnCancelPerformed;
        }

        private void OnNavigatePerformed(InputAction.CallbackContext obj) =>
            NavigatePerformed?.Invoke(obj.ReadValue<Vector2>());
        private void OnSubmitPerformed(InputAction.CallbackContext obj) =>
            SubmitPerformed?.Invoke();
        private void OnCancelPerformed(InputAction.CallbackContext obj) =>
            CancelPerformed?.Invoke();
        #endregion
    }
}