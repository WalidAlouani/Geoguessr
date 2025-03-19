using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CoinEvent", menuName = "ScriptableObjects/Tile Events/Coin Event")]
public class CoinRewardEvent : TileEvent
{
    public int coinAmount = 1000;

    public override void Execute(TileItem tile, IPlayer player, SignalBus signalBus)
    {
        player.AddCoins(coinAmount);

        signalBus.Fire(new CoinsAddedSignal(player, tile.transform.position, coinAmount));

        Debug.Log($"{player.Name} received {coinAmount} coins.");
    }
}
