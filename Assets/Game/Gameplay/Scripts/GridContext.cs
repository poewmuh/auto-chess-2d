using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Game.Gameplay
{
    public class GridContext
    {
        [Inject] private Tilemap _gameTilemap;
        
        private readonly Dictionary<Vector3Int, Entity> _tilesEntities = new (32);

        public bool HasTile(Vector3Int position)
        {
            return _gameTilemap.HasTile(position);
        }

        public BoundsInt.PositionEnumerator GetAllTilePositions()
        {
            return _gameTilemap.cellBounds.allPositionsWithin;
        }

        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return _gameTilemap.WorldToCell(worldPosition);
        }

        public Vector3 GetWorldCellCenterPosition(Vector3 worldPosition)
        {
            var cellPos = _gameTilemap.WorldToCell(worldPosition);
            var centerCellWorld = _gameTilemap.GetCellCenterWorld(cellPos);
            return centerCellWorld;
        }
        
        public Vector3 GetCellCenterWorld(Vector3Int cellPosition)
        {
            return _gameTilemap.GetCellCenterWorld(cellPosition);
        }

        public void SetTileColor(Vector3Int position, Color color)
        {
            _gameTilemap.SetColor(position, color);
        }

        public void RegisterTileEntity(Vector3Int position, Entity entity)
        {
            _tilesEntities[position] = entity;
        }

        public bool TryGetTileEntity(Vector3Int position, out Entity entity)
        {
            return _tilesEntities.TryGetValue(position, out entity);
        }
    }
}