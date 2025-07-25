using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem
{
    [Serializable]
    public sealed class WindowsNavigator : IWindowsNavigator
    {
        private const int VIEW_STACK_DEFAULT_CAPACITY = 6;
        private const int LOADED_VIEWS_DEFAULT_CAPACITY = 6;

        private readonly Stack<Window> viewStack = new(VIEW_STACK_DEFAULT_CAPACITY);
        private List<Window> cachedViews = new(LOADED_VIEWS_DEFAULT_CAPACITY);

        private Window activeView;

        private readonly IWindowsFactory navigationViewFactory;

        [Inject]
        public WindowsNavigator(IWindowsFactory navigationViewFactory) =>
            this.navigationViewFactory = navigationViewFactory;

        public async UniTask InitializeAsync(params string[] addresses)
        {
            if (addresses == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{this.GetType()} can't initialize, cause addresses count <= 0");
#endif
                return;
            }

            for (int i = 0; i < addresses.Length; i++)
            {
                Window view = await navigationViewFactory.CreateAsync(addresses[i]);
                cachedViews.Add(view);
                view.Hide();
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < cachedViews.Count; i++)
                cachedViews[i].Hide();
        }

        public void CleanUp()
        {
            for (int i = 0; i < viewStack.Count; i++)
                viewStack.Pop();
            viewStack.Clear();
        }

        public Window GetActiveWindow() => activeView;

        public void Show<T>() where T : Window
        {
            Type targetType = typeof(T);
            for (int i = 0; i < cachedViews.Count; i++)
            {
                if (ReferenceEquals(cachedViews[i].GetType(), targetType))
                {
                    activeView?.Hide();
                    activeView = cachedViews[i];
                    viewStack.Push(activeView);
                    activeView.Show();
                    break;
                }
            }
        }

        public void ShowPopUp<T>() where T : PopUpWindow
        {
            int orderToDisplay = 0;
            Type targetType = typeof(T);
            for (int i = 0; i < cachedViews.Count; i++)
            {
                if (ReferenceEquals(cachedViews[i].GetType(), targetType))
                {
                    orderToDisplay = activeView?.SortOrder ?? 0 + 1;
                    PopUpWindow window = cachedViews[i] as PopUpWindow;
                    window.ChangeDisplayOrder(orderToDisplay);
                    cachedViews[i].Show();
                    activeView = cachedViews[i];
                    viewStack.Push(cachedViews[i]);
                    break;
                }
            }
        }

        public void Pop()
        {
            if (viewStack.Count < 2) return;

            if (viewStack.TryPop(out Window view))
            {
                activeView.Hide();
                if (viewStack.TryPeek(out Window last))
                {
                    activeView = last;
                    last.Show();
                    Debug.Log($"Peek window {last.name}");
                }
            }
        }

        public void Dispose()
        {
            CleanUp();
        }

        public T ShowAndGetPopUp<T>() where T : PopUpWindow
        {
            int orderToDisplay = 0;
            Type targetType = typeof(T);

            if (ReferenceEquals(activeView.GetType(), targetType))
            {
                activeView.Show();
                return (T)activeView;
            }

            for (int i = 0; i < cachedViews.Count; i++)
            {
                if (ReferenceEquals(cachedViews[i].GetType(), targetType))
                {
                    orderToDisplay = activeView?.SortOrder ?? 0 + 1;
                    T window = cachedViews[i] as T;
                    window.ChangeDisplayOrder(orderToDisplay);
                    cachedViews[i].Show();
                    activeView = cachedViews[i];
                    viewStack.Push(cachedViews[i]);
                    return window;
                }
            }
            return default;
        }
    }
}
