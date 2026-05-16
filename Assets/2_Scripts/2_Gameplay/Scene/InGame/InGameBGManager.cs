using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace JumpRabbit.GamePlay.InGame
{
    public class InGameBGManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SpriteRenderer _bgTile;
        [SerializeField] private Vector2 _tileSize = new Vector2(37.5f, 16.5f);
        [SerializeField] private int _keepRangeX = 2;
        [SerializeField] private int _keepRangeY = 0;

        private Vector2Int _currentCenterCoord = new(int.MinValue, int.MinValue);
        private readonly Dictionary<Vector2Int, SpriteRenderer> _tiles = new();

        private void LateUpdate()
        {
            Vector2Int cameraCoord = GetCameraTileCoord();

            if (cameraCoord == _currentCenterCoord)
                return;

            _currentCenterCoord = cameraCoord;
            RefreshTilesAround(cameraCoord);
        }

        private Vector2Int GetCameraTileCoord()
        {
            Vector3 position = _camera.transform.position;

            return new Vector2Int(
                Mathf.FloorToInt(position.x / _tileSize.x),
                Mathf.FloorToInt(position.y / _tileSize.y));
        }

        private void RefreshTilesAround(Vector2Int centerCoord)
        {
            for (int x = centerCoord.x - _keepRangeX; x <= centerCoord.x + _keepRangeX; x++)
            {
                for (int y = centerCoord.y - _keepRangeY; y <= centerCoord.y + _keepRangeY; y++)
                {
                    Vector2Int coord = new(x, y);

                    if (_tiles.ContainsKey(coord))
                        continue;

                    CreateTile(coord);
                }
            }

            ReleaseFarTiles(centerCoord);
        }

        private void CreateTile(Vector2Int coord)
        {
            Vector3 spawnPosition = new Vector3(coord.x * _tileSize.x, coord.y * _tileSize.y, 0);

            SpriteRenderer tile = Instantiate(_bgTile, spawnPosition, Quaternion.identity);

            _tiles[coord] = tile;
        }

        private void ReleaseFarTiles(Vector2Int centerCoord)
        {
            List<Vector2Int> removeTargets = new();

            foreach (Vector2Int coord in _tiles.Keys)
            {
                if (Mathf.Abs(coord.x - centerCoord.x) > _keepRangeX + 1 ||
                    Mathf.Abs(coord.y - centerCoord.y) > _keepRangeY + 1)
                {
                    removeTargets.Add(coord);
                }
            }

            foreach (Vector2Int coord in removeTargets)
            {
                Destroy(_tiles[coord].gameObject);
                _tiles.Remove(coord);
            }
        }
    }

}
