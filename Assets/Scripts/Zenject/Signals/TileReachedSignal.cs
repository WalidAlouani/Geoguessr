public class TileReachedSignal
{
    public Player Player { get; private set; }
    public int TileIndex { get; private set; }

    public TileReachedSignal(Player player, int tileIndex)
    {
        Player = player;
        TileIndex = tileIndex;
    }
}
