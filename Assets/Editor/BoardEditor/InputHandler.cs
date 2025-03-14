using UnityEngine;
using System.Linq;
using System;

namespace Tools.BoardEditor
{
    public class InputHandler
    {
        private bool isDragging = false;
        private Rect gridRect;
        private float tileSize;
        private TileType tileType;
        private LoopingPath loopingPath;

        public InputHandler(LoopingPath loopingPath)
        {
            this.loopingPath = loopingPath;
        }

        public void UpdateParameters(Rect gridRect, float tileSize, TileType currentSelectedTileType)
        {
            this.gridRect = gridRect;
            this.tileSize = tileSize;
            this.tileType = currentSelectedTileType;
        }

        public void ProcessInput(Event e, int gridWidth, int gridHeight)
        {
            if (!gridRect.Contains(e.mousePosition))
                return;

            Vector2Int tilePos = GetTilePosition(e.mousePosition, gridWidth, gridHeight);

            if (tilePos.x >= gridWidth || tilePos.x < 0 || tilePos.y >= gridHeight)
                return;

            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0) // Left click.
                {
                    if (loopingPath.IsLoopClosed)
                    {
                        loopingPath.ChangeType(tilePos, tileType);
                        e.Use();
                        return;
                    }
                    isDragging = true;
                    if (loopingPath.Path.Count == 0)
                    {
                        loopingPath.TryAddTile(tilePos);
                    }
                    else if (tilePos == loopingPath.Path[0] && loopingPath.Path.Count >= 3)
                    {
                        loopingPath.TryAddTile(tilePos);
                    }
                    e.Use();
                }
                else if (e.button == 1) // Right click.
                {
                    if (loopingPath.Path.Contains(tilePos))
                    {
                        loopingPath.RemoveTilesAfter(tilePos);
                        e.Use();
                    }
                }
            }
            else if (e.type == EventType.MouseDrag && isDragging)
            {
                if (loopingPath.IsLoopClosed)
                {
                    e.Use();
                    return;
                }

                // Undo by dragging back onto the second-to-last tile.
                if (loopingPath.Path.Count > 1 &&
                    tilePos == loopingPath.Path[loopingPath.Path.Count - 2])
                {
                    loopingPath.UndoLastTile();
                    e.Use();
                }
                else if (!loopingPath.Path.Contains(tilePos))
                {
                    Vector2Int last = loopingPath.Path[loopingPath.Path.Count - 1];
                    if (LoopingPath.IsAdjacent(last, tilePos))
                    {
                        loopingPath.TryAddTile(tilePos);
                        e.Use();
                    }
                }
            }
            else if (e.type == EventType.MouseUp && isDragging)
            {
                isDragging = false;
                e.Use();
            }
        }

        private Vector2Int GetTilePosition(Vector2 mousePos, int gridWidth, int gridHeight)
        {
            float totalGridWidth = gridWidth * tileSize; // Total width of the grid

            // Centering the grid within the available editor window space
            float startX = (gridRect.width - totalGridWidth) / 2;

            int x = (int)((mousePos.x - gridRect.x - startX) / tileSize);
            int y = (int)((mousePos.y - gridRect.y) / tileSize);
            return new Vector2Int(x, y);
        }
    }
}
