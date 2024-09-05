using System;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Content.Infrastructure.SceneManagement
{
    public interface ISceneLoader
    {
        public UniTask<SceneInstance> LoadScene(SceneName sceneName, Action<SceneName> onLoaded = null);
    }
}