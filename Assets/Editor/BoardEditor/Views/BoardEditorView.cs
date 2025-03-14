using UnityEngine;
using UnityEditor;
using System;

namespace Tools.BoardEditor
{
    public class BoardEditorView : IBoardEditorView
    {
        private int gridWidth = 10;
        private int gridHeight = 10;
        private float tileSize = 40f;
        private Rect gridRect;
        private Vector2 scrollPos;
        private TileType currentSelectedTileType;

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
            loopingPath.SetData(controller.CurrentBoard.Tiles);
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

            RenderTileTypeButtons();

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
            inputHandler.UpdateParameters(gridRect, tileSize, currentSelectedTileType);

            // Process input (now fully delegated to the InputHandler).
            inputHandler.ProcessInput(Event.current, gridWidth, gridHeight);

            // Render the grid.
            gridRenderer.DrawGrid(loopingPath);

            GUILayout.Space(10);

            if (loopingPath.IsLoopClosed)
            {
                GUILayout.Label("Loop is closed!", EditorStyles.boldLabel);
            }

            RenderControlButtons();

            EditorGUILayout.EndScrollView();
        }

        private void RenderTileTypeButtons()
        {
            if (!loopingPath.IsLoopClosed)
                return;

            GUILayout.Space(10);

            GUILayout.Label("Select Tile Type:", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();

            for (int i = 0; i < Enum.GetValues(typeof(TileType)).Length; i++)
            {
                var enumValue = (TileType)i;
                if (GUILayout.Button(enumValue.ToString()))
                {
                    currentSelectedTileType = enumValue;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void RenderControlButtons()
        {
            GUILayout.Space(10);

            if (loopingPath.IsLoopClosed && GUILayout.Button("Save Board"))
            {
                controller.CurrentBoard.Tiles = loopingPath.GetData();
                controller.SaveBoard();
            }

            if (GUILayout.Button("Back"))
            {
                changeView.Invoke(BoardEditorScreen.BoardList);
            }
        }
    }
}