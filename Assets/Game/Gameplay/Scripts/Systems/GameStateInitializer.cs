using Game.Gameplay.Components;
using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;

namespace Game.Gameplay.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GameStateInitializer : IInitializer
    {
        private Event<GameStateEvent> _gameStateEvent;
        
        private StartBattleComponent _startBattleComponent;
        private Stash<GameStateComponent> _gameStateStash;
        private Entity _gameStateEntity;
        
        public World World { get; set; }

        public void OnAwake()
        {
            _gameStateStash = World.GetStash<GameStateComponent>();
            _gameStateEntity = World.CreateEntity();
            ref var gameStateComp = ref _gameStateStash.Add(_gameStateEntity);
            gameStateComp.gameState = GameState.Preparation;
            
            _gameStateEvent = World.GetEvent<GameStateEvent>();
            _gameStateEvent.NextFrame(new GameStateEvent(GameState.Preparation));

            var startBattleStash = World.GetStash<StartBattleComponent>();
            var filter = World.Filter.With<StartBattleComponent>().Build();
            _startBattleComponent = startBattleStash.Get(filter.First());
            _startBattleComponent.SubscribeOnButton(OnStartBattle);
        }

        private void OnStartBattle()
        {
            ref var gameStateComponent = ref _gameStateStash.Get(_gameStateEntity);
            gameStateComponent.gameState = GameState.Battle;
            _gameStateEvent.NextFrame(new GameStateEvent(GameState.Battle));
        }

        public void Dispose()
        {
            _startBattleComponent.UnsubscribeFromButton(OnStartBattle);
        }
    }
}