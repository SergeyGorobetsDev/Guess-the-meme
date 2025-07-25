using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Assets.Code.Scripts.Runtime.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        private const int Assets_Capacity = 40;

        private readonly Dictionary<string, AsyncOperationHandle> completedCache = new(Assets_Capacity);
        private readonly Dictionary<string, List<AsyncOperationHandle>> handles = new(Assets_Capacity);

        public AssetProvider() { }

        public async UniTask InitializeAsync() =>
            await Addressables.InitializeAsync();

        public Task<GameObject> InstantiateAsync(string address) =>
            Addressables.InstantiateAsync(address).Task;

        public Task<GameObject> InstantiateAsync(string address, Vector3 at) =>
            Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public async UniTask LoadSceneAsync(string address)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(address, LoadSceneMode.Additive, false);
            completedCache.Add(address, handle);

            SceneInstance scene = await handle.Task;

            if (handle.IsDone)
                await scene.ActivateAsync();
        }

        public async UniTask LoadSceneAsync(string address, LoadSceneMode loadSceneMode = LoadSceneMode.Additive, bool activateOnLoad = false)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(address, loadSceneMode, activateOnLoad);
            completedCache.Add(address, handle);

            SceneInstance scene = await handle.Task;

            if (handle.IsDone)
                await scene.ActivateAsync();
        }

        public async UniTask UnloadSceneAsync(string address)
        {
            if (completedCache.TryGetValue(address, out AsyncOperationHandle handle))
            {
                await Addressables.UnloadSceneAsync(handle);
                completedCache.Remove(address);
            }

            await UniTask.CompletedTask;
        }

        public async Task<T> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            if (completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference),
                cacheKey: assetReference.AssetGUID);
        }

        public async Task<T> LoadAsync<T>(string address) where T : class
        {
            if (completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(address),
                cacheKey: address);
        }

        public bool IsLoaded(string address) =>
            completedCache.TryGetValue(address, out AsyncOperationHandle handle);

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);

            completedCache.Clear();
            handles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle =>
                                completedCache[cacheKey] = completeHandle;

            AddHandle(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string cacheKey, AsyncOperationHandle<T> handle) where T : class
        {
            if (!handles.TryGetValue(cacheKey, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                handles[cacheKey] = resourceHandles;

                resourceHandles.Add(handle);
            }
        }
    }
}