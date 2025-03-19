public class TeleportCommand : ICommand
{
    private IPlayer player;
    private TileItem toTile;
    private BoardManager boardManager;
    private CommandQueue commandQueue;

    public TeleportCommand(IPlayer player, TileItem toTile, BoardManager boardManager, CommandQueue queue)
    {
        this.player = player;
        this.toTile = toTile;
        this.boardManager = boardManager;
        this.commandQueue = queue;
    }

    public void Execute()
    {
        var path = boardManager.GetTilesForPlayerMovement(player.Index, toTile);
        player.Controller.TeleportTo(path);
        commandQueue.CommandFinished();
    }
}

