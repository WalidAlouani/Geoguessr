public class TileStoppedSignal
{
    public Player Player { get; private set; }
    public int TileIndex { get; private set; }

    public TileStoppedSignal(Player player, int tileIndex)
    {
        Player = player;
        TileIndex = tileIndex;
    }
}
