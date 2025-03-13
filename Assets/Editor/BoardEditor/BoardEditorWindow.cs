using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Tools.BoardEditor
{
    public class BoardEditorWindow : EditorWindow
    {
        [SerializeField] private BoardEditorSO config;

        private Dictionary<BoardEditorScreen, IBoardEditorView> viewMapping;

        private BoardEditorScreen currentView = BoardEditorScreen.BoardList;

        [MenuItem("Tools/Board Editor")]
        public static void ShowWindow()
        {
            GetWindow<BoardEditorWindow>("Board Editor");
        }

        private void OnEnable()
        {
            var controller = new BoardEditorController(config);
            viewMapping = new Dictionary<BoardEditorScreen, IBoardEditorView>()
            {
                { BoardEditorScreen.BoardList, new BoardListView(controller, ChangeView) },
                { BoardEditorScreen.BoardEditor, new BoardEditorView(controller, ChangeView) },
                { BoardEditorScreen.CreateBoard, new CreateBoardView(controller, ChangeView) },
            };
        }

        private void OnGUI()
        {
            if (config == null)
            {
                GUILayout.Label("No config found! Please create a config file.");
                return;
            }

            viewMapping[currentView].OnRender();

            if (GUI.changed)
                Repaint();
        }

        public void ChangeView(BoardEditorScreen view)
        {
            viewMapping[currentView].OnExit();
            currentView = view;
            viewMapping[currentView].OnEnter();
        }
    }
}
