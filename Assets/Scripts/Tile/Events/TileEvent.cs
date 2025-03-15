using System;
using UnityEngine;

public abstract class TileEvent: ScriptableObject
{
    public abstract void Execute(Player player, Action onEventComplete);
}