
using UnityEngine;

public class CoinsAddedSignal
{
    public Player Player { get; }
    public Vector3 Position { get; }
    public int CoinAmount { get; }

    public CoinsAddedSignal(Player player, Vector3 position, int coinAmount)
    {
        Player = player;
        Position = position;
        CoinAmount = coinAmount;
    }
}
