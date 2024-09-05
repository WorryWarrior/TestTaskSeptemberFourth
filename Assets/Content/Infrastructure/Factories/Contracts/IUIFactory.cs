using System.Collections.Generic;
using Content.Gameplay.Code.Stats.Contracts;
using Content.UI.Code.CharacterStatView;
using Content.UI.Code.InventoryView;
using Content.UI.Code.LoadingView;
using Content.UI.Code.SettingsView;
using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.Factories.Contracts
{
    public interface IUIFactory
    {
        UniTask WarmUp();
        void CleanUp();
        UniTask CreateUIRoot();
        UniTask<LoadingView> CreateOrGetLoadingView();
        UniTask CreateJoystickView();
        UniTask<CharacterStatView> CreateCharacterStatView(IEnumerable<StatBase> stats);
        UniTask<SettingsView> CreateSettingsView();

        UniTask<InventoryHUDController> CreateInventoryView();
        UniTask<InventorySlotController> CreateInventorySlot();
        UniTask<InventoryDragPreviewController> CreateInventoryDragPreview();
    }
}