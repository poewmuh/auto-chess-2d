using Game.Gameplay.Systems.Grid;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Game.Gameplay.Boot
{
    public class GameInitializer : MonoBehaviour
    {
        private World _world;
        private Tilemap _gameTilemap;

        [Inject]
        public void Construct(Tilemap gameTilemap)
        {
            _gameTilemap = gameTilemap;
        }

        private void Start()
        {
            _world = World.Default;

            var systemGroup = _world.CreateSystemsGroup();

            systemGroup.AddInitializer(new TileHightlightSystem(_gameTilemap));
            systemGroup.AddSystem(new CursorGridSystem(_gameTilemap));
            
            _world.AddSystemsGroup(order: 0, systemGroup);
        }
    }
}