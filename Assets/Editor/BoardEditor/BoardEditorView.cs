using UnityEngine;
using UnityEditor;
using Tools.BoardEditor;
using System;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class BoardEditorView : IBoardEditorView
{
    private int gridWidth = 10;
    private int gridHeight = 10;
    private float tileSize = 40f;
    private Rect gridRect;
    private Vector2 scrollPos;

    private LoopingPath loopingPath;
    private GridRenderer gridRenderer;
    private InputHandler inputHandler;

    private BoardEditorController controller;
    private BoardEditorSO config;
    private Action<BoardEditorScreen> changeView;

    public BoardEditorView(BoardEditorController controller, Action<BoardEditorScreen> changeView)
    {
        this.controller = controller;
        this.changeView = changeView;
        config = controller.Config;

        loopingPath = new LoopingPath();
        gridRenderer = new GridRenderer(gridWidth, gridHeight, tileSize);
        inputHandler = new InputHandler(loopingPath);

        tileSize = config.TileSize;
    }

    public void OnEnter()
    {
        var tilesData = controller.CurrentBoard.Tiles.Select(el => new Vector2Int(el.Position.X, el.Position.Y)).ToList();
        loopingPath.SetPath(tilesData);
    }

    public void OnExit()
    {
    }

    public void OnRender()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Looping Form Tool", EditorStyles.boldLabel);
        gridWidth = EditorGUILayout.IntSlider("Grid Width", gridWidth, config.MinGridWidth, config.MaxGridWidth);
        gridHeight = EditorGUILayout.IntSlider("Grid Height", gridHeight, config.MinGridHeight, config.MaxGridHeight);

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

        RenderControlButtons();

        EditorGUILayout.EndScrollView();
    }

    private void RenderControlButtons()
    {
        GUILayout.Space(10);

        if (loopingPath.IsLoopClosed && GUILayout.Button("Save Board"))
        {
            controller.CurrentBoard.Tiles = loopingPath.Path.Select(el => new TileData() { Position = new Coordinates() { X = el.x, Y = el.y } }).ToList();
            controller.SaveBoard();

            SaveScriptableObject();
        }

        if (GUILayout.Button("Back"))
        {
            changeView.Invoke(BoardEditorScreen.BoardList);
        }
    }


    private void SaveScriptableObject()
    {
        var assetPath = $"Assets/ScriptableObjects/BoardData/BoardData{controller.CurrentBoard.Id}.asset";

        var boardDataSO = AssetDatabase.LoadAssetAtPath<BoardDataSO>(assetPath);
        if (boardDataSO == null)
        {
            boardDataSO = ScriptableObject.CreateInstance<BoardDataSO>();
            AssetDatabase.CreateAsset(boardDataSO, assetPath);
        }

        boardDataSO.SetTiles(controller.CurrentBoard.Tiles);
        EditorUtility.SetDirty(boardDataSO);
        AssetDatabase.SaveAssets();
    }
}