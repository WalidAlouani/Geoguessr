using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class BoardSerializerJson : IDataSerializer<BoardData>
{
    public string FileExtension => "json";

    public void Save(string path, BoardData Board)
    {
        string json = JsonConvert.SerializeObject(Board);
        File.WriteAllText(path, json);
        Debug.Log($"Saved Board: {path}");
    }

    public BoardData Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Board to load not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<BoardData>(json);
    }
}
