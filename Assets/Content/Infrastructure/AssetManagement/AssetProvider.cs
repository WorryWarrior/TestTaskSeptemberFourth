using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Content.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Initialize() => Addressables.InitializeAsync();

        public async UniTask<T> Load<T>(string key) where T : class
        {
            if (_completedCache.TryGetValue(key, out var completedHandle))
                return completedHandle.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

            return await RunWithCacheOnComplete(handle, key);
        }

        public async UniTask<IList<T>> LoadByLabel<T>(string label) where T : class
        {
            if (_completedCache.TryGetValue(label, out var completedHandle))
                return completedHandle.Result as IList<T>;

            var handle = Addressables.LoadAssetsAsync<T>(label, null);

            return await RunWithCacheOnComplete(handle, label);
        }

        public void Release(string key)
        {
            if (!_handles.ContainsKey(key))
                return;

            foreach (AsyncOperationHandle handle in _handles[key])
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }

        public void Cleanup()
        {
            if (_handles.Count == 0)
                return;

            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);

            _completedCache.Clear();
            _handles.Clear();
        }

        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle => _completedCache[cacheKey] = completeHandle;

            AddHandle(cacheKey, handle);
            return await handle.Task;
        }

        private async UniTask<IList<T>> RunWithCacheOnComplete<T>(AsyncOperationHandle<IList<T>> handle, string label)
        {
            await handle.Task;

            _completedCache[label] = handle;

            if (!_handles.ContainsKey(label))
                _handles[label] = new List<AsyncOperationHandle>();

            _handles[label].Add(handle);

            return handle.Result;
        }

        private void AddHandle(string key, AsyncOperationHandle handle)
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }
    }
}