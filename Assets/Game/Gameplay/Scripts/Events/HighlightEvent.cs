using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Events
{
    public struct HighlightEvent : IEventData
    {
        public Vector3Int mapPosition;
    }
}