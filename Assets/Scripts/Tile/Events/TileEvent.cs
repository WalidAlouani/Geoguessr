using System;
using UnityEngine;
using Zenject;

public abstract class TileEvent: ScriptableObject
{
    public abstract void Execute(TileItem tile, Player player, SignalBus signalBus);
}