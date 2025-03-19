using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class QuizSerializerJson : IDataSerializer<QuizData>
{
    public string FileExtension => "json";

    public void Save(string path, QuizData level)
    {
        string json = JsonConvert.SerializeObject(level);
        File.WriteAllText(path, json);
        Debug.Log($"Saved quiz: {path}");
    }

    public QuizData Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Quiz to load not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<QuizData>(json);
    }

    public static QuizData LoadFromJson(string json)
    {
        return JsonConvert.DeserializeObject<QuizData>(json);
    }
}
