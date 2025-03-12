using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class LoopingFormEditor : EditorWindow
{
    private int gridWidth = 10;
    private int gridHeight = 10;
    private float tileSize = 40f;
    private Rect gridRect;

    private LoopingPath loopingPath;
    private GridRenderer gridRenderer;
    private InputHandler inputHandler;

    [MenuItem("Tools/Looping Form Editor")]
    public static void ShowWindow()
    {
        GetWindow<LoopingFormEditor>("Looping Form Editor");
    }

    private void OnEnable()
    {
        loopingPath = new LoopingPath();
        gridRenderer = new GridRenderer(gridWidth, gridHeight, tileSize);
        inputHandler = new InputHandler(loopingPath);
    }

    private void OnGUI()
    {
        GUILayout.Label("Looping Form Tool", EditorStyles.boldLabel);
        gridWidth = EditorGUILayout.IntField("Grid Width", gridWidth);
        gridHeight = EditorGUILayout.IntField("Grid Height", gridHeight);
        tileSize = EditorGUILayout.FloatField("Tile Size", tileSize);

        if (GUILayout.Button("Reset"))
        {
            loopingPath.Reset();
        }

        // Reserve space for the grid.
        gridRect = GUILayoutUtility.GetRect(gridWidth * tileSize, gridHeight * tileSize);

        // In LoopingFormEditor.OnGUI():
        loopingPath.TrimToBounds(gridWidth, gridHeight);

        // Update grid and input parameters.
        gridRenderer.UpdateParameters(gridWidth, gridHeight, tileSize, gridRect);
        inputHandler.UpdateParameters(gridRect, tileSize);

        // Process input (now fully delegated to the InputHandler).
        inputHandler.ProcessInput(Event.current, gridWidth, gridHeight);

        // Render the grid.
        gridRenderer.DrawGrid(loopingPath);

        GUILayout.Space(10);
        GUILayout.Label("Current Path:");
        string pathStr = "";
        foreach (Vector2Int pos in loopingPath.Path)
        {
            pathStr += $"({pos.x},{pos.y}) ";
        }
        GUILayout.Label(pathStr);

        if (loopingPath.IsLoopClosed)
        {
            GUILayout.Label("Loop is closed!", EditorStyles.boldLabel);
        }

        if (GUI.changed)
            Repaint();
    }
}

public class LoopingPath
{
    private List<Vector2Int> _path = new List<Vector2Int>();
    public IReadOnlyList<Vector2Int> Path => _path;

    public bool IsLoopClosed => _path.Count >= 3 && _path[0] == _path[_path.Count - 1];

    public bool TryAddTile(Vector2Int tile)
    {
        if (IsLoopClosed) return false;
        if (_path.Count == 0 || (tile == _path[0] && _path.Count >= 3))
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
                    if (IsCurrentTile(path, tilePos))
                    {
                        EditorGUI.DrawRect(tileRect, Color.yellow);
                    }
                    else
                    {
                        EditorGUI.DrawRect(tileRect, new Color(0.5f, 1f, 0.5f, 0.5f));
                    }
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
}

public class InputHandler
{
    private bool isDragging = false;
    private Rect gridRect;
    private float tileSize;
    private LoopingPath loopingPath;

    public InputHandler(LoopingPath loopingPath)
    {
        this.loopingPath = loopingPath;
    }

    /// <summary>
    /// Updates the input handler with the latest grid parameters.
    /// </summary>
    public void UpdateParameters(Rect gridRect, float tileSize)
    {
        this.gridRect = gridRect;
        this.tileSize = tileSize;
    }

    /// <summary>
    /// Processes mouse input events.
    /// </summary>
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
