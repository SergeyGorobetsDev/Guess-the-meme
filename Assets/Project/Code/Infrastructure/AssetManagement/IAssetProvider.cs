using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Assets.Code.Scripts.Runtime.AssetManagement
{
    public interface IAssetProvider
    {
        UniTask InitializeAsync();
        Task<GameObject> InstantiateAsync(string address);
        Task<GameObject> InstantiateAsync(string address, Vector3 at);
        UniTask LoadSceneAsync(string address);
        UniTask UnloadSceneAsync(string address);
        Task<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        Task<T> LoadAsync<T>(string address) where T : class;
        void CleanUp();
        UniTask LoadSceneAsync(string address, LoadSceneMode loadSceneMode = LoadSceneMode.Additive, bool activateOnLoad = false);
        bool IsLoaded(string address);
    }
}