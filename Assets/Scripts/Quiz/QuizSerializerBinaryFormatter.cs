using System.IO;
using UnityEngine;
// Not recommended due to security vulnerabilities
using System.Runtime.Serialization.Formatters.Binary;

public class LevelSerializerBinaryFormatter : IDataSerializer<QuizData>
{
    public string FileExtension => "data";

    public void Save(string path, QuizData level)
    {
        var formatter = new BinaryFormatter();

        var stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, level);
        stream.Close();

        Debug.Log($"Saved level: {path}");
    }

    public QuizData Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Level to load not found: {path}");
            return null;
        }

        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Open);
        var levelData = formatter.Deserialize(stream) as QuizData;
        stream.Close();
        return levelData;
    }
}
