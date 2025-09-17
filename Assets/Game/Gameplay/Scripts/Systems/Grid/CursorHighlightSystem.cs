using System;
using Game.Gameplay.Components.Grid;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Game.Gameplay.Systems.Grid
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorHighlightSystem : IInitializer
    {
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
            _subscription = cursorMapTileEvent.Subscribe(OnCursorTileChanged);
            _highlightStash = World.GetStash<HighlightComponent>();
        }

        private void OnCursorTileChanged(FastList<CursorMapTileEvent> triggers)
        {
            var last = triggers[triggers.length - 1];
            var newPos = last.mapPosition;

            if (_lastHighlightedPos.HasValue)
            {
                var prevPos = _lastHighlightedPos.Value;
                if (_gridContext.TryGetTileEntity(prevPos, out var prevEntity) && _highlightStash.Has(prevEntity))
                {
                    _highlightStash.Remove(prevEntity);
                }
                _lastHighlightedPos = null;
            }
            
            if (!last.isValidPosition)
                return;
            
            if (_gridContext.TryGetTileEntity(newPos, out var tileEntity))
            {
                if (!_highlightStash.Has(tileEntity))
                {
                    _highlightStash.Add(tileEntity) = new HighlightComponent
                    {
                        position = newPos,
                        type = HighlightType.Cursor
                    };
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