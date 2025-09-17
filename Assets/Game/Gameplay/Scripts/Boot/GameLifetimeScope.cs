using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay.Boot
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Tilemap _gameTileMap;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GridContext>(Lifetime.Singleton);
            builder.RegisterInstance(_gameTileMap);
        }
    }
}