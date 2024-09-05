using Content.Gameplay.Code.Camera;
using Content.Gameplay.Code.Items;
using Content.Gameplay.Code.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Infrastructure.Factories.Contracts
{
    public interface ILevelFactory
    {
        UniTask WarmUp();
        void CleanUp();
        UniTask<LevelController> CreateLevel();
        UniTask<CameraController> CreateCamera();
        UniTask<GameObject> CreateCharacter();
        UniTask<SceneItemController> CreateRandomItem();
    }
}