using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private IPlayer player;
    private List<Vector3> steps;
    private CommandQueue commandQueue;

    public MoveCommand(IPlayer player, List<Vector3> steps, CommandQueue queue)
    {
        this.player = player;
        this.steps = steps;
        this.commandQueue = queue;
    }

    public async void Execute()
    {
        await player.Controller.MoveSteps(steps);
        OnCommandFinished();
    }

    private void OnCommandFinished()
    {
        commandQueue.CommandFinished();
    }
}

