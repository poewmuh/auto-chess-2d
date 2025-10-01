using System;
using Game.Gameplay.Components;
using Game.Gameplay.Components.Unit;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Gameplay.Systems.Unit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SelectionSystem : ISystem
    {
        private Entity _selectedUnit;
        private Stash<MovableComponent> _movableStash;
        private Stash<PositionComponent> _positionStash;
        private Stash<EnemyMarker> _enemyStash;
        private Stash<SelectedMarker> _selectedStash;
        private Filter _units;
        private IDisposable _subscription;
        
        public World World { get; set; }

        public void OnAwake()
        {
            var cursorClickEvent = World.GetEvent<CursorMapClickEvent>();
            _subscription = cursorClickEvent.Subscribe(OnMapClick);

            _movableStash = World.GetStash<MovableComponent>();
            _positionStash = World.GetStash<PositionComponent>();
            _selectedStash = World.GetStash<SelectedMarker>();
            _enemyStash = World.GetStash<EnemyMarker>();
            _units = World.Filter.With<PositionComponent>().Build();
            
        }

        private bool IsBattleState()
        {
            var startBattleFilter = World.Filter.With<GameStateComponent>().Build();
            var startBattleStash = World.GetStash<GameStateComponent>();
            ref var stateComponent = ref startBattleStash.Get(startBattleFilter.First());
            return stateComponent.gameState == GameState.Battle;
        }

        private void OnMapClick(FastList<CursorMapClickEvent> triggers)
        {
            if (IsBattleState()) return;
            var lastTrigger = triggers[triggers.length - 1];

            if (_selectedStash.IsEmpty())
            {
                TrySelectUnit(lastTrigger.mapPosition);
            }
            else
            {
                TryAddToMovable(lastTrigger.mapPosition);
            }
        }

        private void TrySelectUnit(Vector3Int mapPosition)
        {
            foreach (var entity in _units)
            {
                if (_enemyStash.Has(entity)) continue;
                ref var positionComponent = ref _positionStash.Get(entity);
                if (positionComponent.position != mapPosition) continue;
                
                _selectedUnit = entity;
                _selectedStash.Add(entity);
                return;
            }
        }

        private void TryAddToMovable(Vector3Int mapPosition)
        {
            foreach (var entity in _units)
            {
                ref var positionComponent = ref _positionStash.Get(entity);
                if (positionComponent.position != mapPosition) continue;
                
                _selectedStash.RemoveAll();
                return;
            }
            
            _movableStash.Add(_selectedUnit) = new MovableComponent() { movePosition = mapPosition};
            _selectedStash.Remove(_selectedUnit);
        }

        public void OnUpdate(float deltaTime) { }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}