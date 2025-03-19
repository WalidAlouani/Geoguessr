
using UnityEngine;

public class CoinsAddedSignal
{
    public IPlayer Player { get; }
    public Vector3 Position { get; }
    public int CoinAmount { get; }

    public CoinsAddedSignal(IPlayer player, Vector3 position, int coinAmount)
    {
        Player = player;
        Position = position;
        CoinAmount = coinAmount;
    }
}
