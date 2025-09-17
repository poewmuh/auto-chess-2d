using Game.Gameplay.Components;
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
        private Stash<TransformComponent> _transformStash;

        public World World { get; set; }
        
        public UnitInitializer(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            _units = World.Filter.With<PositionComponent>().With<TransformComponent>().Build();
            _positionStash = World.GetStash<PositionComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            
            foreach (var entity in _units)
            {
                ref var positionComp = ref _positionStash.Get(entity);
                ref var transformComp = ref _transformStash.Get(entity);
                
                positionComp.position = _gridContext.WorldToCell(transformComp.GetPosition());
                transformComp.transform.position = _gridContext.GetCellCenterWorld(positionComp.position);
            }
        }

        public void Dispose()
        {

        }
    }
}