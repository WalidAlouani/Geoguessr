public class CoinsUpdateSignal
{
    public IPlayer Player { get; private set; }

    public CoinsUpdateSignal(IPlayer player)
    {
        Player = player;
    }
}
