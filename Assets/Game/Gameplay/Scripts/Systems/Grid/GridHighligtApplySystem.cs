using Game.Gameplay.Components.Grid;
using Game.Gameplay.Helpers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Gameplay.Systems.Grid
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridHighligtApplySystem : ISystem
    {
        public World World { get; set; }

        private Tilemap _gameMap;
        private Stash<HighlightComponent> _highlightStash;
        private Stash<TileComponent> _tileStash;
        private Filter _tileFilter;

        public GridHighligtApplySystem(Tilemap tilemap)
        {
            _gameMap = tilemap;
        }

        public void OnAwake()
        {
            _highlightStash = World.GetStash<HighlightComponent>();
            _tileStash = World.GetStash<TileComponent>();
            _tileFilter = World.Filter.With<TileComponent>().Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var tileEntity in _tileFilter)
            {
                var tileComponent = _tileStash.Get(tileEntity);
                var color = ColorsHelper.DEFAULT_TILE_COLOR;
                if (_highlightStash.Has(tileEntity))
                {
                    var highlightComponent = _highlightStash.Get(tileEntity);
                    color = GetColorByComponent(highlightComponent);
                }
                

                _gameMap.SetColor(tileComponent.tilePos, color);
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

        public void Dispose()
        {

        }
    }
}