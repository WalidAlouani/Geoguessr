using UnityEngine;
using UnityEditor;
using System;

namespace Tools.BoardEditor
{
    public class CreateBoardView : IBoardEditorView
    {
        private BoardEditorController controller;
        private readonly Action<BoardEditorScreen> changeView;

        private BoardData currentBoard;

        public CreateBoardView(BoardEditorController controller, Action<BoardEditorScreen> changeView)
        {
            this.controller = controller;
            this.changeView = changeView;
        }

        public void OnEnter()
        {
            controller.CreateBoard();
            currentBoard = controller.CurrentBoard;
        }

        public void OnExit()
        {
            GUI.FocusControl(null);
        }

        public void OnRender()
        {
            GUILayout.Space(10);
            GUILayout.Label("Create New Board", EditorStyles.boldLabel);

            currentBoard.Id = EditorGUILayout.IntField("Number", currentBoard.Id);

            // Save Button
            if (GUILayout.Button("Create Board"))
            {
                if (controller.OverwriteCheck())
                {
                    bool overwrite = EditorUtility.DisplayDialog("Board already exist", $"A file called 'Board{currentBoard.Id}' already exists.\nDo you want to overwrite it?", "Yes", "No");
                    if (!overwrite)
                        return;
                }
                controller.SaveBoard();
                currentBoard.Id++;
            }

            if (GUILayout.Button("Back"))
            {
                changeView.Invoke(BoardEditorScreen.BoardList);
            }
        }
    }
}