using System;
using UnityEngine;
using Zenject;

public abstract class TileEvent: ScriptableObject
{
    public abstract void Execute(TileItem tile, IPlayer player, SignalBus signalBus);
}