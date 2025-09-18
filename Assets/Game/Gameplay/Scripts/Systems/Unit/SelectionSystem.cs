using System;
using Game.Gameplay.Components;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace Game.Gameplay.Systems.Unit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SelectionSystem : ISystem
    {
        private Stash<PositionComponent> _positionStash;
        private Stash<SelectedMarker> _selectedStash;
        private Filter _units;
        private IDisposable _subscription;
        
        public World World { get; set; }

        public void OnAwake()
        {
            var cursorClickEvent = World.GetEvent<CursorMapClickEvent>();
            _subscription = cursorClickEvent.Subscribe(OnMapClick);

            _positionStash = World.GetStash<PositionComponent>();
            _selectedStash = World.GetStash<SelectedMarker>();
            _units = World.Filter.With<PositionComponent>().With<SelectedMarker>().Build();
        }

        private void OnMapClick(FastList<CursorMapClickEvent> triggers)
        {
            var lastTrigger = triggers[triggers.length - 1];
            foreach (var entity in _units)
            {
                ref var posComp = ref _positionStash.Get(entity);

                if (posComp.position == lastTrigger.mapPosition)
                {
                    _selectedStash.Add(entity);
                }
                else
                {
                    _selectedStash.Remove(entity);
                }
            }
        }

        public void OnUpdate(float deltaTime) { }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}