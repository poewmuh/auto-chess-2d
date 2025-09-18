using System;
using Game.Gameplay.Components.Grid;
using Game.Gameplay.Events;
using Game.Gameplay.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Gameplay.Systems.Grid
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridHighlightApplySystem : ISystem
    {
        private IDisposable _subscribtion;
        public World World { get; set; }

        private readonly GridContext _gridContext;
        private Stash<HighlightComponent> _highlightStash;

        public GridHighlightApplySystem(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            _highlightStash = World.GetStash<HighlightComponent>();
            
            var highlightEvent = World.GetEvent<HighlightEvent>();
            _subscribtion = highlightEvent.Subscribe(OnHighlightChange);
        }

        private void OnHighlightChange(FastList<HighlightEvent> triggers)
        {
            foreach (var t in triggers)
            {
                _gridContext.TryGetTileEntity(t.mapPosition, out var tileEntity);
                var color = ColorsHelper.DEFAULT_TILE_COLOR;
                if (_highlightStash.Has(tileEntity))
                {
                    ref var highlightComponent = ref _highlightStash.Get(tileEntity);
                    color = GetColorByComponent(highlightComponent);
                }
                

                _gridContext.SetTileColor(t.mapPosition, color);
            }
        }

        private Color GetColorByComponent(HighlightComponent component)
        {
            switch (component.type)
            {
                case HighlightType.Cursor:
                    return ColorsHelper.HIGHLIGHT_TILE_COLOR;
                case HighlightType.Selected:
                    return ColorsHelper.SELECTED_TILE_COLOR;
            }
            
            return ColorsHelper.DEFAULT_TILE_COLOR;
        }
        
        public void OnUpdate(float deltaTime) { }

        public void Dispose()
        {
            _subscribtion.Dispose();
        }
    }
}