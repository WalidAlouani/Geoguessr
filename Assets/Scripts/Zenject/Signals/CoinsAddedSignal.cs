public class CoinsAddedSignal
{
    public Player Player { get; }
    public TileItem Tile { get; }
    public int CoinAmount { get; }

    public CoinsAddedSignal(Player player, TileItem tile, int coinAmount)
    {
        Player = player;
        Tile = tile;
        CoinAmount = coinAmount;
    }
}
