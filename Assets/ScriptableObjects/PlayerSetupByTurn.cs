using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetupByTurn", menuName = "ScriptableObjects/PlayerSetupByTurn", order = 6)]
public class PlayerSetupByTurn : ScriptableObject
{
    public List<PlayerSetupEntry> entries;
}


[Serializable]
public class PlayerSetupEntry
{
    public Material material;
    public Color color;
}