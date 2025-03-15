public class PlayerStartMoveSignal
{
    public PlayerController Player { get; private set; }

    public PlayerStartMoveSignal(PlayerController player)
    {
        Player = player;
    }
}