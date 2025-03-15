public class TurnStartedSignal
{
    public PlayerController Player { get; private set; }

    public TurnStartedSignal(PlayerController player)
    {
        Player = player;
    }
}
