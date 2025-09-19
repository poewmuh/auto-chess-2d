using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay.Components.Unit
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SelectedMarker : IComponent { }
}