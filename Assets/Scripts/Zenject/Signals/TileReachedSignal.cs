public class TileReachedSignal
{
    public IPlayer Player { get; private set; }
    public int TileIndex { get; private set; }

    public TileReachedSignal(IPlayer player, int tileIndex)
    {
        Player = player;
        TileIndex = tileIndex;
    }
}
