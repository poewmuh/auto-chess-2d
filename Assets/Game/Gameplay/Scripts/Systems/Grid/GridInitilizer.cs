using Game.Gameplay.Components.Grid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Tilemaps;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class GridInitilizer : IInitializer 
{
    public World World { get; set;}
    
    private Tilemap _tilemap;

    public GridInitilizer(Tilemap gameGrid)
    {
        _tilemap = gameGrid;
    }

    public void OnAwake() 
    {
        var stash = World.GetStash<TileComponent>();
        foreach (var tilePos in _tilemap.cellBounds.allPositionsWithin)
        {
            if (!_tilemap.HasTile(tilePos)) continue;
                
            var entity = World.CreateEntity();
            ref var tile = ref stash.Add(entity);
            tile.tilePos = tilePos;
        }
    }

    public void Dispose()
    {

    }
}