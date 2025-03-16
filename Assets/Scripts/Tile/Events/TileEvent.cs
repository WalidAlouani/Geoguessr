using System;
using UnityEngine;

public abstract class TileEvent: ScriptableObject
{
    public abstract void Execute(TileItem tile, Player player, Action onEventComplete);
}