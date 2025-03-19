using System;
using System.Collections;
using UnityEngine;

public class WaitCommand : ICommand
{
    private IPlayer player;
    private float time;
    private CommandQueue commandQueue;

    public WaitCommand(IPlayer player, float time, CommandQueue queue)
    {
        this.player = player;
        this.time = time;
        this.commandQueue = queue;
    }

    public void Execute()
    {
        player.Controller.StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        OnCommandFinished();
    }

    private void OnCommandFinished()
    {
        commandQueue.CommandFinished();
    }
}