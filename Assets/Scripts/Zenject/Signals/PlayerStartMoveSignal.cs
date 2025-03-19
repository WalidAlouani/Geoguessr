public class PlayerStartMoveSignal
{
    public IPlayer Player { get; private set; }

    public PlayerStartMoveSignal(IPlayer player)
    {
        Player = player;
    }
}
