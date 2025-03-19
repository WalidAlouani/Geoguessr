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

    public void Execute()
    {
        player.Controller.OnMoveComplete += OnCommandFinished;
        player.Controller.MoveSteps(steps);
    }

    private void OnCommandFinished()
    {
        player.Controller.OnMoveComplete -= OnCommandFinished;
        commandQueue.CommandFinished();
    }
}

