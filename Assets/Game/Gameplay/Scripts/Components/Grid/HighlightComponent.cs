using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Gameplay.Components.Grid
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HighlightComponent : IComponent
    {
        public Vector3Int position;
        public HighlightType type;
    }

    public enum HighlightType
    {
        None = 0,
        Cursor = 1,
        Selected = 2
    }
}