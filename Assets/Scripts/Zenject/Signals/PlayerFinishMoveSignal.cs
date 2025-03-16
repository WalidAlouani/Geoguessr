public class PlayerFinishMoveSignal
{
    public Player Player { get; private set; }

    public PlayerFinishMoveSignal(Player player)
    {
        Player = player;
    }
}