using UnityEngine;
using System;

public class CoinRewardEvent : ITileEvent
{
    private int coinAmount;

    public CoinRewardEvent(int amount)
    {
        coinAmount = amount;
    }

    public void Execute(Player player, Action onEventComplete)
    {
        player.AddCoins(coinAmount);
        Debug.Log($"{player.Name} received {coinAmount} coins.");
        onEventComplete?.Invoke(); // Notify that the event is complete
    }
}
