using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Events
{
    public struct CursorMapClickEvent : IEventData
    {
        public Vector3Int mapPosition;
    }
}