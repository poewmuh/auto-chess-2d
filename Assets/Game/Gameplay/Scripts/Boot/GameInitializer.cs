using Game.Gameplay.Systems.Grid;
using Game.Gameplay.Systems.Unit;
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

            systemGroup.AddInitializer(new GridInitilizer(_gameTilemap));
            systemGroup.AddInitializer(new UnitInitializer(_gameTilemap));
            systemGroup.AddInitializer(new CursorHightlightSystem());

            systemGroup.AddSystem(new GridHighligtApplySystem(_gameTilemap));
            systemGroup.AddSystem(new CursorGridSystem(_gameTilemap));
            systemGroup.AddSystem(new SelectionSystem());
            
            _world.AddSystemsGroup(order: 0, systemGroup);
        }
    }
}