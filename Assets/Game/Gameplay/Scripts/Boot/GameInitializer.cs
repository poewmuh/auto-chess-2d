using Game.Gameplay.Systems;
using Game.Gameplay.Systems.Battle;
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

            var preparationGroup = _world.CreateSystemsGroup();

            preparationGroup.AddInitializer(new GameStateInitializer());
            preparationGroup.AddInitializer(new GridInitializer(_gridContext));
            preparationGroup.AddInitializer(new UnitInitializer(_gridContext));
            preparationGroup.AddInitializer(new CursorHighlightSystem(_gridContext));
            
            preparationGroup.AddSystem(new SelectionHighlightSystem(_gridContext));
            preparationGroup.AddSystem(new GridHighlightApplySystem(_gridContext));
            preparationGroup.AddSystem(new CursorGridSystem(_gridContext));
            preparationGroup.AddSystem(new SelectionSystem());
            preparationGroup.AddSystem(new MovableSystem(_gridContext));
            
            var battleGroup = _world.CreateSystemsGroup();
            battleGroup.AddSystem(new FindTargetSystem());
            battleGroup.AddSystem(new BattleMoveSystem(_gridContext));
            
            _world.AddSystemsGroup(order: 0, preparationGroup);
            _world.AddSystemsGroup(order: 1, battleGroup);
        }
    }
}