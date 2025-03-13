using System.Collections.Generic;

namespace Tools.BoardEditor
{
    public class BoardEditorController
    {
        public BoardEditorSO Config { get; private set; }
        public List<string> BoardFiles { get; private set; }
        public BoardData CurrentBoard { get; private set; }

        private BoardFileHandler fileHandler;

        public BoardEditorController(BoardEditorSO config)
        {
            var BoardSerializer = new BoardSerializerJson();
            fileHandler = new BoardFileHandler(BoardSerializer, config.SaveDirectory);
            Config = config;
            RefreshBoardList();
        }

        public void RefreshBoardList()
        {
            BoardFiles = fileHandler.GetBoardsNames();
        }

        public void LoadBoard(string fileName)
        {
            CurrentBoard = fileHandler.LoadBoard(fileName);
        }

        public void SaveBoard()
        {
            if (CurrentBoard == null)
                return;

            fileHandler.SaveBoard(CurrentBoard);
            RefreshBoardList();
        }

        public void DeleteBoard(string fileName)
        {
            fileHandler.DeleteBoard(fileName);
            RefreshBoardList();
        }

        public void CreateBoard()
        {
            var BoardNumber = fileHandler.MaxBoardNumber() + 1;
            CurrentBoard = new BoardData(BoardNumber);
        }

        public bool OverwriteCheck()
        {
            if (CurrentBoard == null)
                return false;

            return fileHandler.IsBoardFileExist(CurrentBoard);
        }
    }
}