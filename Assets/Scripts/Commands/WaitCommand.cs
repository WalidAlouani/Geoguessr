using System;
using System.Collections;
using UnityEngine;

public class WaitCommand : ICommand
{
    private Player player;
    private float time;
    private Action onCommandComplete;
    private CommandQueue commandQueue;

    public WaitCommand(Player player, float time, CommandQueue queue, Action onCommandComplete = null)
    {
        this.player = player;
        this.time = time;
        this.onCommandComplete = onCommandComplete;
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
        onCommandComplete?.Invoke();
    }
}