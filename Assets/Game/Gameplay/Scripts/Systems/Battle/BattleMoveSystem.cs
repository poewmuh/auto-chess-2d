using System;
using Game.Gameplay.Components.Battle;
using Game.Gameplay.Components.Unit;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay.Systems.Battle
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleMoveSystem : ISystem
    {
        private Filter _unitsWithTarget;
        private Stash<PositionComponent> _positions;
        private Stash<TargetComponent> _targetStash;
        private readonly GridContext _gridContext;
        
        private const float TEST_TIMER = 0.4f;
        private float _timer;
        
        public World World { get; set; }

        public BattleMoveSystem(GridContext gridContext)
        {
            _gridContext = gridContext;
        }

        public void OnAwake()
        {
            _unitsWithTarget = World.Filter.With<PositionComponent>().With<TargetComponent>().Build();
            _targetStash = World.GetStash<TargetComponent>();
            _positions = World.GetStash<PositionComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timer < TEST_TIMER)
            {
                _timer += deltaTime;
                return;
            }

            _timer = 0;
            foreach (var unitEntity in _unitsWithTarget) 
            {
                ref var pos = ref _positions.Get(unitEntity);
                ref var target = ref _targetStash.Get(unitEntity);

                var tPos = _positions.Get(target.targetEntity);
                var dx = tPos.position.x - pos.position.x;
                var dy = tPos.position.y - pos.position.y;
                
                var distSqr = dx * dx + dy * dy;
                var attackRange = 1;

                if (distSqr <= attackRange * attackRange) continue;

                if (Math.Abs(dx) > Math.Abs(dy)) 
                {
                    pos.MoveCellPositionX(Math.Sign(dx), _gridContext);
                }
                else 
                {
                    pos.MoveCellPositionY(Math.Sign(dy), _gridContext);
                }
            }
        }

        public void Dispose()
        {

        }
    }
}