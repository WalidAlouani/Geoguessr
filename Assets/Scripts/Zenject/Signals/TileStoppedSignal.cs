public class TileStoppedSignal
{
    public IPlayer Player { get; private set; }
    public int TileIndex { get; private set; }

    public TileStoppedSignal(IPlayer player, int tileIndex)
    {
        Player = player;
        TileIndex = tileIndex;
    }
}
