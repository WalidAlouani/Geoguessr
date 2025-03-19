public class TurnStartedSignal
{
    public IPlayer Player { get; private set; }

    public TurnStartedSignal(IPlayer player)
    {
        Player = player;
    }
}
