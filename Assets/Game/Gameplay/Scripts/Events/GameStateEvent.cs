using Game.Gameplay.Components;
using Scellecs.Morpeh;

namespace Game.Gameplay.Events
{
    public struct GameStateEvent : IEventData
    {
        public GameStateEvent(GameState gameState)
        {
            newState = gameState;
        }
        
        public GameState newState;
    }
}