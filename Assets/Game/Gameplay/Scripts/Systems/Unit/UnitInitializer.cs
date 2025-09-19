using Game.Gameplay.Components.Unit;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay.Systems.Unit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnitInitializer : IInitializer
    {
        private readonly GridContext _gridContext;
        
        private Filter _units;
        private Stash<PositionComponent> _positionStash;

        public World World { get; set; }
        
        public UnitInitializer(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            _units = World.Filter.With<PositionComponent>().Build();
            _positionStash = World.GetStash<PositionComponent>();
            
            foreach (var entity in _units)
            {
                ref var positionComp = ref _positionStash.Get(entity);

                var centerCell = _gridContext.GetWorldCellCenterPosition(positionComp.GetWorldPosition());
                positionComp.SetWorldPosition(centerCell, _gridContext);
            }
        }

        public void Dispose()
        {

        }
    }
}