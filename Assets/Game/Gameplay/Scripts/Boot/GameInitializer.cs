using Game.Gameplay.Systems.Grid;
using Game.Gameplay.Systems.Unit;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Game.Gameplay.Boot
{
    public class GameInitializer : MonoBehaviour
    {
        private World _world;
        [Inject] private GridContext _gridContext;

        private void Start()
        {
            _world = World.Default;

            var systemGroup = _world.CreateSystemsGroup();

            systemGroup.AddInitializer(new GridInitializer(_gridContext));
            systemGroup.AddInitializer(new UnitInitializer(_gridContext));
            systemGroup.AddInitializer(new CursorHighlightSystem(_gridContext));

            systemGroup.AddSystem(new GridHighlightApplySystem(_gridContext));
            systemGroup.AddSystem(new CursorGridSystem(_gridContext));
            systemGroup.AddSystem(new SelectionSystem());
            
            _world.AddSystemsGroup(order: 0, systemGroup);
        }
    }
}