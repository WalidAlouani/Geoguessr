using UnityEngine;

[CreateAssetMenu(fileName = "BoardEditorConfig", menuName = "ScriptableObjects/BoardEditorConfig", order = 1)]
public class BoardEditorSO : ScriptableObject
{
    [Header("Storage Section")]
    public string SaveDirectoryXML;
    public string SaveDirectorySO;
    //public BoardSerializerType SerializerType;

    [Header("Grid Section")]
    public int MinGridWidth;
    public int MinGridHeight;
    public int MaxGridWidth;
    public int MaxGridHeight;
    [Range(20f, 60f)]
    public int TileSize;
}
