using UnityEngine;
using System;
using Zenject;

[CreateAssetMenu(fileName = "CoinEvent", menuName = "ScriptableObjects/Tile Events/Coin Event")]
public class CoinRewardEvent : TileEvent
{
    public int coinAmount = 1000;
    public UI_FloatingText floatingText;

    public override void Execute(TileItem tile, Player player, SignalBus signalBus)
    {
        player.AddCoins(coinAmount);

        // ToDo: replace later with a Factory with pooling
        var obj = Instantiate(floatingText, tile.transform.position, Quaternion.identity);
        obj.Init(coinAmount);

        Debug.Log($"{player.Name} received {coinAmount} coins.");
    }
}
