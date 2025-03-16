using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CoinEvent", menuName = "ScriptableObjects/Tile Events/Coin Event")]
public class CoinRewardEvent : TileEvent
{
    public int coinAmount = 1000;

    public override void Execute(TileItem tile, Player player, SignalBus signalBus)
    {
        player.AddCoins(coinAmount);

        signalBus.Fire(new CoinsAddedSignal(player, tile, coinAmount));

        Debug.Log($"{player.Name} received {coinAmount} coins.");
    }
}
