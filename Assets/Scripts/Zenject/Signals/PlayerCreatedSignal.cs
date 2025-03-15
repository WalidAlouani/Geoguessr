public class PlayerCreatedSignal
{
    public Player Player { get; private set; }

    public PlayerCreatedSignal(Player player)
    {
        Player = player;
    }
}
