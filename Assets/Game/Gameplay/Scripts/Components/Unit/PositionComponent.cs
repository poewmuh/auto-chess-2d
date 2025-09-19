using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Gameplay.Components.Unit
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PositionComponent : IComponent
    {
        public Transform transform;
        public Vector3Int position;

        public Vector3 GetWorldPosition() => transform.position;

        public void SetWorldPosition(Vector3 worldPosition, GridContext gridContext)
        {
            transform.position = worldPosition;
            position = gridContext.WorldToCell(worldPosition);
        }
    }
}