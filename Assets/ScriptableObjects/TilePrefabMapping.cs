using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TilePrefabMapping", menuName = "ScriptableObjects/TilePrefabMapping", order = 4)]
public class TilePrefabMapping : ScriptableObject
{
    public List<TilePrefabEntry> entries;
}

[Serializable]
public class TilePrefabEntry
{
    public TileType tileType;
    public TileItem tilePrefab;
}
