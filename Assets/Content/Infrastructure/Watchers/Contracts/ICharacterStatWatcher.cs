using System.Collections.Generic;
using Content.Gameplay.Code.Stats.Contracts;

namespace Content.Infrastructure.Watchers.Contracts
{
    public interface ICharacterStatWatcher
    {
        void Initialize(IEnumerable<StatBase> characterStats);
    }
}