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
    public sealed class CursorHightlightSystem : IInitializer
    {
        private IDisposable _subscription;

        private Stash<HighlightComponent> _highlightStash;
        private Stash<TileComponent> _tileStash;
        private Filter _tileFilter;
        
        public World World { get; set; }

        public void OnAwake()
        {
            var cursorMapTileEvent = World.GetEvent<CursorMapTileEvent>();
            _subscription = cursorMapTileEvent.Subscribe(OnCursorTileChanged);
            _tileStash = World.GetStash<TileComponent>();
            _tileFilter = World.Filter.With<TileComponent>().Build();
            _highlightStash = World.GetStash<HighlightComponent>();
        }

        private void OnCursorTileChanged(FastList<CursorMapTileEvent> triggers)
        {
            foreach (var t in triggers)
            {
                foreach (var tileEntity in _tileFilter)
                {
                    var tileComponent = _tileStash.Get(tileEntity);
                    if (t.isValidPosition && tileComponent.tilePos == t.mapPosition && !_highlightStash.Has(tileEntity))
                    {
                        Debug.Log($"[CursorHighlightSystem] Add Highlight in {t.mapPosition}");
                        _highlightStash.Add(tileEntity) = new HighlightComponent() {  type = HighlightType.Cursor };
                    }
                    else
                    {
                        _highlightStash.Remove(tileEntity);
                    }
                }
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}