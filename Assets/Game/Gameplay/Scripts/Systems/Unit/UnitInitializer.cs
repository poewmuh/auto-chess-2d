using Game.Gameplay.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Tilemaps;

namespace Game.Gameplay.Systems.Unit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnitInitializer : IInitializer
    {
        private readonly Tilemap _gameMap;
        private Filter _units;

        public World World { get; set; }

        public UnitInitializer(Tilemap gameMap)
        {
            _gameMap = gameMap;
        }

        public void OnAwake()
        {
            _units = World.Filter.With<PositionComponent>().With<TransformComponent>().Build();
            
            foreach (var entity in _units)
            {
                ref var posComp = ref entity.GetComponent<PositionComponent>();
                ref var tComp = ref entity.GetComponent<TransformComponent>();
                
                posComp.position = _gameMap.WorldToCell(tComp.transform.position);
                tComp.transform.position = _gameMap.GetCellCenterWorld(posComp.position);
            }
        }

        public void Dispose()
        {

        }
    }
}