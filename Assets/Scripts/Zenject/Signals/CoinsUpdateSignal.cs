public class CoinsUpdateSignal
{
    public Player Player { get; private set; }

    public CoinsUpdateSignal(Player player)
    {
        Player = player;
    }
}
