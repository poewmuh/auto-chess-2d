using Game.Gameplay.Components.Grid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridInitializer : IInitializer
    {
        public World World { get; set; }

        private readonly GridContext _gridContext;

        public GridInitializer(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            var stash = World.GetStash<TileComponent>();
            foreach (var tilePos in _gridContext.GetAllTilePositions())
            {
                if (!_gridContext.HasTile(tilePos)) continue;

                var entity = World.CreateEntity();
                ref var tile = ref stash.Add(entity);
                tile.position = tilePos;
                _gridContext.RegisterTileEntity(tilePos, entity);
            }
        }

        public void Dispose()
        {

        }
    }
}