using Assets.Code.Scripts.Runtime.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem
{
    public interface IWindowsFactory
    {
        UniTask<Window> CreateAsync(string adress);
    }

    public sealed class WindowsFactory : IWindowsFactory
    {
        private readonly DiContainer container;
        private readonly IAssetProvider assetProvider;

        [Inject]
        public WindowsFactory(DiContainer container, AssetProvider assetProvider)
        {
            this.container = container;
            this.assetProvider = assetProvider;
        }

        public async UniTask<Window> CreateAsync(string adress)
        {
            GameObject view = await assetProvider.LoadAsync<GameObject>(adress);

            if (view == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{this.GetType()} can't load Window with address {adress}");
#endif
                return null;
            }

            var window = container.InstantiatePrefabForComponent<Window>(view);
            return window;
        }
    }
}