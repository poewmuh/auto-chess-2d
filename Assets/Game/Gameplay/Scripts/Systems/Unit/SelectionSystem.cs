using System;
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

        private void OnMapClick(FastList<CursorMapClickEvent> triggers)
        {
            var lastTrigger = triggers[triggers.length - 1];
            foreach (var entity in _units)
            {
                ref var posComp = ref _positionStash.Get(entity);

                if (posComp.position == lastTrigger.mapPosition)
                {
                    if (!_selectedStash.Has(entity))
                    {
                        if (!_enemyStash.Has(entity))
                        {
                            _selectedUnit = entity;
                            _selectedStash.Add(entity);
                        }
                        
                        return;
                    }
                    else
                    {
                        _selectedStash.Remove(entity);
                    }
                }
            }

            AddToMovable(lastTrigger.mapPosition);
        }

        private void AddToMovable(Vector3Int mapPosition)
        {
            ref var selectedUnitPos = ref _positionStash.Get(_selectedUnit);
            if (_selectedStash.Has(_selectedUnit) && mapPosition != selectedUnitPos.position)
            {
                _movableStash.Add(_selectedUnit) = new MovableComponent() { movePosition = mapPosition};
                _selectedStash.Remove(_selectedUnit);
            }
        }

        public void OnUpdate(float deltaTime) { }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}