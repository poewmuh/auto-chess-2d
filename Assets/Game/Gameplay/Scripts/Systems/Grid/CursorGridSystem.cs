using Game.Gameplay.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Gameplay.Systems.Grid
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorGridSystem : ISystem
    {
        private Event<CursorMapTileEvent> _cursorMapTileEvent;
        private Event<CursorMapClickEvent> _cursorMapClickEvent;
        
        private Camera _camera;
        private Vector3Int _lastGridPos;
        private bool _isOnValidPos;
        
        private readonly Tilemap _gameTilemap;
        
        public World World { get; set; }

        public CursorGridSystem(Tilemap gameGrid)
        {
            _gameTilemap = gameGrid;
        }

        public void OnAwake()
        {
            _camera = Camera.main;
            _lastGridPos = Vector3Int.up * 100;
            _cursorMapTileEvent = World.GetEvent<CursorMapTileEvent>();
            _cursorMapClickEvent = World.GetEvent<CursorMapClickEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z);
            var mouseWorldPos = _camera.ScreenToWorldPoint(mousePos);
            var cellPos = _gameTilemap.WorldToCell(mouseWorldPos);

            ProcessCursorOnMap(cellPos);
            ProcessCursorClick(cellPos);
        }

        private void ProcessCursorOnMap(Vector3Int cellPos)
        {
            if (_lastGridPos == cellPos) return;
            _lastGridPos = cellPos;
            if (!_gameTilemap.HasTile(cellPos))
            {
                _isOnValidPos = false;
                _cursorMapTileEvent.NextFrame(new CursorMapTileEvent
                {
                    isValidPosition = false
                });
                return;
            }

            _isOnValidPos = true;
            _cursorMapTileEvent.NextFrame(new CursorMapTileEvent
            {
                mapPosition = cellPos,
                isValidPosition = true
            });
        }

        private void ProcessCursorClick(Vector3Int cellPos)
        {
            if (!_isOnValidPos) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                _cursorMapClickEvent.NextFrame(new CursorMapClickEvent
                {
                    mapPosition = cellPos
                });
            }
        }

        public void Dispose()
        {

        }
    }
}