using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class TeleportCommand : ICommand
{
    private PlayerController player;
    private TileItem toTile;
    private CommandQueue commandQueue;

    public TeleportCommand(PlayerController player, TileItem toTile, CommandQueue queue)
    {
        this.player = player;
        this.toTile = toTile;
        this.commandQueue = queue;
    }

    public void Execute()
    {
        player.Teleport(toTile);
        commandQueue.CommandFinished();
    }
}

