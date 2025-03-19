public class TurnEndedSignal
{
    public IPlayer Player { get; private set; }

    public TurnEndedSignal(IPlayer player)
    {
        Player = player;
    }
}
