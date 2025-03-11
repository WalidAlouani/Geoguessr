using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuizFileHandler
{
    private string directory;
    private IDataSerializer<QuizData> serializer;

    public QuizFileHandler(IDataSerializer<QuizData> serializer, string directory)
    {
        this.serializer = serializer;
        this.directory = Path.Combine(Application.streamingAssetsPath, directory);

        if (!Directory.Exists(this.directory))
        {
            Directory.CreateDirectory(this.directory);
        }
    }

    public bool IsFileExist(QuizData quiz)
    {
        string path = Path.Combine(directory, $"quiz{quiz.ID}." + serializer.FileExtension);
        return File.Exists(path);
    }

    public QuizData Load(string filename)
    {
        string path = Path.Combine(directory, filename);
        return serializer.Load(path);
    }

    public QuizData Load(int quizNumber)
    {
        string path = Path.Combine(directory, $"quiz{quizNumber}." + serializer.FileExtension);
        return serializer.Load(path);
    }

    public void Save(QuizData quiz)
    {
        string path = Path.Combine(directory, $"quiz{quiz.ID}." + serializer.FileExtension);
        serializer.Save(path, quiz);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void Delete(string filename)
    {
        string path = Path.Combine(directory, filename);
        if (!File.Exists(path))
        {
            Debug.LogWarning("quiz to delete not found: " + path);
            return;
        }

        File.Delete(path);
        File.Delete(path + ".meta");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        Debug.Log("quiz deleted: " + path);
    }
}
