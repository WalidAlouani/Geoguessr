using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Tools.BoardEditor
{
    public class LoopingPath
    {
        private List<Vector2Int> _path = new List<Vector2Int>();
        private List<TileType> _types = new List<TileType>();
        public IReadOnlyList<Vector2Int> Path => _path;
        public IReadOnlyList<TileType> Types => _types;

        public bool IsLoopClosed => _path.Count >= 3 && _path[0] == _path[_path.Count - 1];

        public void SetData(List<TileData> tiles)
        {
            _path = tiles.Select(el => new Vector2Int(el.Position.X, el.Position.Y)).ToList();
            _types = tiles.Select(el => el.Type).ToList();
        }

        public List<TileData> GetData()
        {
            List<TileData> tiles = new List<TileData>();

            for (int i = 0; i < _path.Count; i++)
            {
                var tileData = new TileData()
                {
                    Position = new Coordinates(_path[i].x, _path[i].y),
                    Type = _types[i],
                };
                tiles.Add(tileData);
            }

            return tiles;
        }

        public bool TryAddTile(Vector2Int tile)
        {
            if (IsLoopClosed) return false;
            if (_path.Count == 0 || (tile == _path[0] && _path.Count >= 3 && IsAdjacent(_path[0], _path[_path.Count - 1])))
            {
                _path.Add(tile);
                _types.Add(tile == _path[0] ? TileType.Home : TileType.Base);
                return true;
            }

            Vector2Int last = _path[_path.Count - 1];
            if (IsAdjacent(last, tile) && !_path.Contains(tile))
            {
                _path.Add(tile);
                _types.Add(TileType.Base);
                return true;
            }
            return false;
        }

        public void UndoLastTile()
        {
            if (_path.Count > 0)
            {
                _path.RemoveAt(_path.Count - 1);
                _types.Add(TileType.Base);
            }
        }

        public void RemoveTilesAfter(Vector2Int tile)
        {
            int index = _path.IndexOf(tile);
            if (index >= 0 && index < _path.Count - 1)
            {
                _path.RemoveRange(index + 1, _path.Count - index - 1);
                _types.RemoveRange(index + 1, _path.Count - index - 1);
            }
        }

        public void ChangeType(Vector2Int position, TileType type)
        {
            int index = _path.IndexOf(position);
            if (index >= 0 && index < _path.Count - 1)
            {
                _types[index] = type;
            }
        }

        public TileType GetType(Vector2Int position)
        {
            int index = _path.IndexOf(position);
            if (index >= 0 && index < _path.Count - 1)
            {
                return _types[index];
            }
            return TileType.Base;
        }

        public void Reset()
        {
            _path.Clear();
            _types.Clear();
        }

        public void TrimToBounds(int width, int height)
        {
            for (int i = 0; i < _path.Count; i++)
            {
                var tile = _path[i];
                if (tile.x >= width || tile.y >= height)
                {
                    _path.RemoveAt(i);
                    _types.RemoveAt(i);
                }
            }
        }

        public static bool IsAdjacent(Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            return (dx + dy == 1);
        }
    }
}
