using UnityEngine;
using System.Collections.Generic;

namespace Tools.BoardEditor
{
    public class LoopingPath
    {
        private List<Vector2Int> _path = new List<Vector2Int>();
        public IReadOnlyList<Vector2Int> Path => _path;

        public bool IsLoopClosed => _path.Count >= 3 && _path[0] == _path[_path.Count - 1];

        public void SetPath(List<Vector2Int> positions)
        {
            _path = positions;
        }

        public bool TryAddTile(Vector2Int tile)
        {
            if (IsLoopClosed) return false;
            if (_path.Count == 0 || (tile == _path[0] && _path.Count >= 3 && IsAdjacent(_path[0], _path[_path.Count - 1])))
            {
                _path.Add(tile);
                return true;
            }

            Vector2Int last = _path[_path.Count - 1];
            if (IsAdjacent(last, tile) && !_path.Contains(tile))
            {
                _path.Add(tile);
                return true;
            }
            return false;
        }

        public void UndoLastTile()
        {
            if (_path.Count > 0)
                _path.RemoveAt(_path.Count - 1);
        }

        public void RemoveTilesAfter(Vector2Int tile)
        {
            int index = _path.IndexOf(tile);
            if (index >= 0 && index < _path.Count - 1)
                _path.RemoveRange(index + 1, _path.Count - index - 1);
        }

        public void Reset()
        {
            _path.Clear();
        }

        public void TrimToBounds(int width, int height)
        {
            _path.RemoveAll(tile => tile.x >= width || tile.y >= height);
        }

        public static bool IsAdjacent(Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            return (dx + dy == 1);
        }
    }
}
