using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Events
{
    public struct CursorMapTileEvent : IEventData
    {
        public bool isValidPosition;
        public Vector3Int mapPosition;
    }
}