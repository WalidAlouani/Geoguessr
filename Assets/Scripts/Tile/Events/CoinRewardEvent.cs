using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CoinEvent", menuName = "ScriptableObjects/Tile Events/Coin Event")]
public class CoinRewardEvent : TileEvent
{
    public int coinAmount = 1000;

    public override void Execute(Player player, Action onEventComplete)
    {
        player.AddCoins(coinAmount);
        Debug.Log($"{player.Name} received {coinAmount} coins.");
        onEventComplete?.Invoke(); // Notify that the event is complete
    }
}
