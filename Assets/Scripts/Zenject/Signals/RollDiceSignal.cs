public class RollDiceSignal
{
    public IPlayer Player { get; private set; }

    public RollDiceSignal(IPlayer player)
    {
        Player = player;
    }
}
