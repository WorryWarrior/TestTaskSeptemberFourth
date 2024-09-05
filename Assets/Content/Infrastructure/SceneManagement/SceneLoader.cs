using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Content.Infrastructure.SceneManagement
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask<SceneInstance> LoadScene(SceneName sceneName, Action<SceneName> onLoaded = null)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName.ToSceneString());
            await handle.Task;
            SceneInstance scene = handle.Result;
            await scene.ActivateAsync();

            onLoaded?.Invoke(sceneName);
            return scene;
        }
    }
}