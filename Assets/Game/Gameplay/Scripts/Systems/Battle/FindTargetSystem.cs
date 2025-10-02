using System;
using Game.Gameplay.Components;
using Game.Gameplay.Components.Battle;
using Game.Gameplay.Components.Unit;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Gameplay.Systems.Battle
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FindTargetSystem : ISystem
    {
        private bool _isBattleState;
        private Filter _unitsFilter;
        private Stash<TargetComponent> _targetStash;
        private Stash<PositionComponent> _positionStash;
        private IDisposable _stateSubscribe;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _unitsFilter = World.Filter.With<PositionComponent>().Build();
            _targetStash = World.GetStash<TargetComponent>();
            _positionStash = World.GetStash<PositionComponent>();

            var stateEvent = World.GetEvent<GameStateEvent>();
            _stateSubscribe = stateEvent.Subscribe(OnStateChange);
        }

        private void OnStateChange(FastList<GameStateEvent> triggers)
        {
            var lastTrigger = triggers[triggers.length - 1];
            _isBattleState = lastTrigger.newState == GameState.Battle;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isBattleState) return;

            foreach (var unitEntity in _unitsFilter)
            {
                if (!_targetStash.Has(unitEntity))
                {
                    ref var unitPosComponent = ref _positionStash.Get(unitEntity);
                    if (TryGetNearestUnit(unitPosComponent.position, out var nearestEntity))
                    {
                        _targetStash.Add(unitEntity) = new TargetComponent()
                        {
                            targetEntity = nearestEntity
                        };
                    }
                }
            }
        }

        private bool TryGetNearestUnit(Vector3Int position, out Entity nearestEntity)
        {
            nearestEntity = new Entity();
            int bestDist = int.MaxValue;
            
            foreach (var unitEntity in _unitsFilter)
            {
                ref var unitPosComponent = ref _positionStash.Get(unitEntity);
                if (position == unitPosComponent.position) continue;
                
                int dist = (position.x - unitPosComponent.position.x) * (position.x - unitPosComponent.position.x) +
                           (position.y - unitPosComponent.position.y) * (position.y - unitPosComponent.position.y);

                if (dist < bestDist) {
                    bestDist = dist;
                    nearestEntity = unitEntity;
                }
            }

            return bestDist < int.MaxValue;
        }

        public void Dispose()
        {
            _stateSubscribe.Dispose();
        }
    }
}