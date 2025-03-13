using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoardFileHandler
{
    private string BoardsDirectory;
    private IDataSerializer<BoardData> serializer;

    public BoardFileHandler(IDataSerializer<BoardData> serializer, string directory)
    {
        this.serializer = serializer;
        BoardsDirectory = Path.Combine(Application.streamingAssetsPath, directory);

        if (!Directory.Exists(BoardsDirectory))
        {
            Directory.CreateDirectory(BoardsDirectory);
        }
    }

    public List<string> GetBoardsNames()
    {
        var files = Directory.GetFiles(BoardsDirectory, "*." + serializer.FileExtension);

        return files.Select(Path.GetFileName)
            .OrderBy(FileUtils.ExtractNumber)
            .ToList();
    }

    public List<int> GetBoardsNumbers()
    {
        var files = Directory.GetFiles(BoardsDirectory, "*." + serializer.FileExtension);

        return files.Select(FileUtils.ExtractNumber)
            .OrderBy(el => el)
            .ToList();
    }

    public bool IsBoardFileExist(BoardData Board)
    {
        string path = Path.Combine(BoardsDirectory, $"Board{Board.Id}." + serializer.FileExtension);
        return File.Exists(path);
    }

    public BoardData LoadBoard(string filename)
    {
        string path = Path.Combine(BoardsDirectory, filename);
        return serializer.Load(path);
    }

    public BoardData LoadBoard(int BoardNumber)
    {
        string path = Path.Combine(BoardsDirectory, $"Board{BoardNumber}." + serializer.FileExtension);
        return serializer.Load(path);
    }

    public void SaveBoard(BoardData Board)
    {
        string path = Path.Combine(BoardsDirectory, $"Board{Board.Id}." + serializer.FileExtension);
        serializer.Save(path, Board);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void DeleteBoard(string filename)
    {
        string path = Path.Combine(BoardsDirectory, filename);
        if (!File.Exists(path))
        {
            Debug.LogWarning("Board to delete not found: " + path);
            return;
        }

        File.Delete(path);
        File.Delete(path + ".meta");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        Debug.Log("Board deleted: " + path);
    }

    public int MaxBoardNumber()
    {
        var BoardFiles = GetBoardsNumbers();
        if (BoardFiles.Count <= 0)
            return 0;

        return BoardFiles.Max();
    }
}
