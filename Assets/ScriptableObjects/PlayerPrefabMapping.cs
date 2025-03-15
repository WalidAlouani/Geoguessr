using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefabMapping", menuName = "ScriptableObjects/PlayerPrefabMapping", order = 4)]
public class PlayerPrefabMapping : ScriptableObject
{
    public List<PlayerPrefabEntry> entries;
}

[Serializable]
public class PlayerPrefabEntry
{
    public PlayerType playerType;
    public GameObject prefab;
}
