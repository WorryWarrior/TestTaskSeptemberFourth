using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Content.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IInitializable
    {
        UniTask<T> Load<T>(string key) where T : class;
        UniTask<IList<T>> LoadByLabel<T>(string label) where T : class;
        void Release(string key);
        void Cleanup();
    }
}