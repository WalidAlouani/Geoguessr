using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CoinEvent", menuName = "ScriptableObjects/Tile Events/Coin Event")]
public class CoinRewardEvent : TileEvent
{
    public int coinAmount = 1000;
    public UI_FloatingText floatingText;

    public override void Execute(TileItem tile, Player player, Action onEventComplete)
    {
        player.AddCoins(coinAmount);

        // ToDo: replace later with a Factory with pooling
        var obj = Instantiate(floatingText, tile.transform.position, Quaternion.identity);
        obj.Init(coinAmount);

        Debug.Log($"{player.Name} received {coinAmount} coins.");
        onEventComplete?.Invoke(); // Notify that the event is complete
    }
}
