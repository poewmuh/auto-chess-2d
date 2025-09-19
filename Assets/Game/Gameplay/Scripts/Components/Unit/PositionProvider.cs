using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay.Components.Unit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PositionProvider : MonoProvider<PositionComponent>
    {
        private void Reset()
        {
            ref PositionComponent positionComponent = ref GetData();
            positionComponent.transform = transform;
        }
    }
}