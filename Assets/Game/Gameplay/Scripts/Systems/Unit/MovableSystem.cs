using Game.Gameplay.Components.Unit;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay.Systems.Unit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MovableSystem : ISystem
    {
        private Filter _movableUnits;
        private Stash<MovableComponent> _movableStash;
        private Stash<PositionComponent> _transformStash;
        private readonly GridContext _gridContext;
        
        public World World { get; set; }

        public MovableSystem(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            _movableStash = World.GetStash<MovableComponent>();
            _transformStash = World.GetStash<PositionComponent>();
            _movableUnits = World.Filter.With<MovableComponent>().Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _movableUnits)
            {
                ref var movableComponent = ref _movableStash.Get(unitEntity);
                ref var transformComponent = ref _transformStash.Get(unitEntity);
                var worldCellCenter = _gridContext.GetCellCenterWorld(movableComponent.movePosition);
                transformComponent.SetWorldPosition(worldCellCenter, _gridContext);
                _movableStash.Remove(unitEntity);
            }
        }

        public void Dispose()
        {

        }
    }
}