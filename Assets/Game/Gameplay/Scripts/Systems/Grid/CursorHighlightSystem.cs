using System;
using Game.Gameplay.Components.Grid;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Gameplay.Systems.Grid
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorHighlightSystem : IInitializer
    {
        private Event<HighlightEvent> _highlightEvent;
        
        private readonly GridContext _gridContext;
        
        private IDisposable _subscription;
        private Stash<HighlightComponent> _highlightStash;
        private Vector3Int? _lastHighlightedPos;

        private Entity _lastHighlightEntity;
        
        public World World { get; set; }
        
        public CursorHighlightSystem(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            var cursorMapTileEvent = World.GetEvent<CursorMapTileEvent>();
            _highlightEvent = World.GetEvent<HighlightEvent>();
            _subscription = cursorMapTileEvent.Subscribe(OnCursorTileChanged);
            _highlightStash = World.GetStash<HighlightComponent>();
        }

        private void OnCursorTileChanged(FastList<CursorMapTileEvent> triggers)
        {
            var lastTrigger = triggers[triggers.length - 1];
            var newPos = lastTrigger.mapPosition;

            RemovePrevHighlight();
            
            if (!lastTrigger.isValidPosition)
                return;

            AddNewHighlight(newPos);
        }

        private void RemovePrevHighlight()
        {
            if (_lastHighlightedPos.HasValue)
            {
                var prevPos = _lastHighlightedPos.Value;
                if (_gridContext.TryGetTileEntity(prevPos, out var prevEntity) && _highlightStash.Has(prevEntity))
                {
                    _highlightStash.Remove(prevEntity);
                    _highlightEvent.NextFrame(new HighlightEvent
                    {
                        mapPosition = prevPos
                    });
                }
                _lastHighlightedPos = null;
            }
        }

        private void AddNewHighlight(Vector3Int newPos)
        {
            if (_gridContext.TryGetTileEntity(newPos, out var tileEntity))
            {
                if (!_highlightStash.Has(tileEntity))
                {
                    _highlightStash.Add(tileEntity) = new HighlightComponent
                    {
                        position = newPos,
                        type = HighlightType.Cursor
                    };
                    
                    _highlightEvent.NextFrame(new HighlightEvent
                    {
                        mapPosition = newPos
                    });
                }

                _lastHighlightedPos = newPos;
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}