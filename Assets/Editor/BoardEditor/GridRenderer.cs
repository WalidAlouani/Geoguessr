using UnityEngine;
using UnityEditor;

public class GridRenderer
{
    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public float TileSize { get; private set; }
    public Rect GridRect { get; private set; }

    public GridRenderer(int gridWidth, int gridHeight, float tileSize)
    {
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        TileSize = tileSize;
    }

    /// <summary>
    /// Updates the grid drawing parameters.
    /// </summary>
    public void UpdateParameters(int gridWidth, int gridHeight, float tileSize, Rect gridRect)
    {
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        TileSize = tileSize;
        GridRect = gridRect;
    }

    /// <summary>
    /// Draws the grid and tiles based on the current path.
    /// </summary>
    public void DrawGrid(LoopingPath path)
    {
        float totalGridWidth = GridWidth * TileSize; // Total width of the grid

        // Centering the grid within the available editor window space
        float startX = (GridRect.width - totalGridWidth) / 2 ;

        for (int x = 0; x < GridWidth; x++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int y = 0; y < GridHeight; y++)
            {
                Rect tileRect = new Rect(startX + GridRect.x + x * TileSize, GridRect.y + y * TileSize, TileSize, TileSize);
                Vector2Int tilePos = new Vector2Int(x, y);

                if (ContainsTile(path, tilePos))
                {
                    int pathIndex = GetTileIndexInPath(path, tilePos); // Get index in path

                    if (IsCurrentTile(path, tilePos))
                    {
                        EditorGUI.DrawRect(tileRect, Color.yellow);
                    }
                    else
                    {
                        EditorGUI.DrawRect(tileRect, new Color(0.5f, 1f, 0.5f, 0.5f));
                    }

                    // Draw order number on the tile
                    Handles.Label(tileRect.center, pathIndex.ToString()); // Display index + 1 for order number
                }
                else
                {
                    EditorGUI.DrawRect(tileRect, Color.gray);
                }

                Handles.DrawSolidRectangleWithOutline(tileRect, Color.clear, Color.black);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    private bool ContainsTile(LoopingPath path, Vector2Int tile)
    {
        foreach (Vector2Int t in path.Path)
        {
            if (t == tile)
                return true;
        }
        return false;
    }

    private bool IsCurrentTile(LoopingPath path, Vector2Int tile)
    {
        return path.Path.Count > 0 && path.Path[path.Path.Count - 1] == tile;
    }

    // Helper function to get the index of a tile in the path
    private int GetTileIndexInPath(LoopingPath path, Vector2Int tile)
    {
        for (int i = 0; i < path.Path.Count; i++)
        {
            if (path.Path[i] == tile)
            {
                return i;
            }
        }
        return -1; // Should not happen if ContainsTile is used correctly before
    }
}
