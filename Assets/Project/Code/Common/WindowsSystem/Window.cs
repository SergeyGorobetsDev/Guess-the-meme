using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem
{
    [Serializable]
    public abstract class Window : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        protected Button closeButton;
        [SerializeField]
        protected Canvas canvas;

        [SerializeField]
        protected bool shouldFocus = false;
        [SerializeField]
        protected Selectable focusElement;

        [Header("States")]
        [SerializeField]
        private bool UseCloseButton = true;

        [Header("States")]
        [SerializeField]
        protected bool uiBinded;
        [SerializeField]
        protected bool callbacksRegistered;

        protected IWindowsNavigator windowsNavigator;

        public int SortOrder => canvas.sortingOrder;

        public virtual void Show()
        {
            this.canvas.enabled = true;
            if (!uiBinded)
                BindDocumentData();

            if (!callbacksRegistered)
                RegisterCallbacks();

            FocusElement();
        }

        public virtual void Hide()
        {
            if (callbacksRegistered)
                UnRegisterCallbacks();
            this.canvas.enabled = false;
        }

        protected virtual void BackButtonPressed() =>
            windowsNavigator.Pop();

        protected virtual void BindDocumentData()
        {
            uiBinded = true;
        }

        protected virtual void RegisterCallbacks()
        {
            if (UseCloseButton && closeButton is not null)
                closeButton.onClick.AddListener(CloseButtonPressed);

            callbacksRegistered = true;
        }

        protected virtual void UnRegisterCallbacks()
        {
            if (UseCloseButton && closeButton is not null)
                closeButton.onClick.RemoveListener(CloseButtonPressed);

            callbacksRegistered = false;
        }

        protected virtual void FocusElement()
        {
            if (shouldFocus && focusElement != null)
                focusElement.Select();
        }

        protected virtual void CloseButtonPressed() =>
            BackButtonPressed();
    }
}