public class TurnStartedSignal
{
    public Player Player { get; private set; }

    public TurnStartedSignal(Player player)
    {
        Player = player;
    }
}
