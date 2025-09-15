using System;
using Game.Gameplay.Events;
using Game.Gameplay.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Gameplay.Systems.Grid
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TileHightlightSystem : IInitializer
    {
        private IDisposable _subscription;

        private Vector3Int? _prevCellPos = null;
        private readonly Tilemap _highlightMap;
        
        public World World { get; set; }

        public TileHightlightSystem(Tilemap highlightMap)
        {
            _highlightMap = highlightMap;
        }

        public void OnAwake()
        {
            var cursorMapTileEvent = World.GetEvent<CursorMapTileEvent>();
            _subscription = cursorMapTileEvent.Subscribe(OnCursorTileChanged);
        }

        private void OnCursorTileChanged(FastList<CursorMapTileEvent> triggers)
        {
            foreach (var t in triggers)
            {
                if (t.mapPosition != _prevCellPos && _prevCellPos != null)
                {
                    _highlightMap.SetColor(_prevCellPos.Value, ColorsHelper.DEFAULT_TILE_COLOR);
                    _prevCellPos = null;
                }
                if (!t.isValidPosition)
                {
                    continue;
                }

                _highlightMap.SetColor(t.mapPosition, ColorsHelper.HIGHLIGHT_TILE_COLOR);
                _prevCellPos = t.mapPosition;
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}