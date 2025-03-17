using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersData", menuName = "ScriptableObjects/PlayersData", order = 7)]
public class PlayerDataList : ScriptableObject
{
    public List<PlayerData> Entries;
}