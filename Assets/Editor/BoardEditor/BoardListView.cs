using UnityEngine;
using UnityEditor;
using System;

namespace Tools.BoardEditor
{
    public class BoardListView : IBoardEditorView
    {
        private BoardEditorController controller;
        private readonly Action<BoardEditorScreen> changeView;
        private Vector2 scrollPos;

        public BoardListView(BoardEditorController controller, Action<BoardEditorScreen> changeView)
        {
            this.controller = controller;
            this.changeView = changeView;
        }

        public void OnEnter() { }

        public void OnExit() { }

        public void OnRender()
        {
            DisplayBoardsList();
            DisplayCreateBoard();
        }

        private void DisplayBoardsList()
        {
            GUILayout.Space(10);
            GUILayout.Label("Boards List", EditorStyles.boldLabel);

            // Refresh Button
            if (GUILayout.Button("Refresh Board List"))
            {
                controller.RefreshBoardList();
            }

            // Display available Board files
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));
            for (int i = 0; i < controller.BoardFiles.Count; i++)
            {
                GUILayout.BeginHorizontal();

                var fileName = controller.BoardFiles[i];

                // Load Board button
                if (GUILayout.Button(fileName, EditorStyles.miniButtonLeft))
                {
                    controller.LoadBoard(fileName);
                    changeView.Invoke(BoardEditorScreen.BoardEditor);
                }

                // Delete Board button
                if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                {
                    if (EditorUtility.DisplayDialog("Delete Board", $"Are you sure you want to delete {fileName}?", "Yes", "No"))
                    {
                        controller.DeleteBoard(fileName);
                    }
                }

                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        private void DisplayCreateBoard()
        {
            // Create New Board
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Board"))
            {
                changeView.Invoke(BoardEditorScreen.CreateBoard);
            }
        }
    }
}
