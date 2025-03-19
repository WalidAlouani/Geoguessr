using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class WaitCommand : ICommand
{
    private IPlayer player;
    private int timeMs;
    private CommandQueue commandQueue;
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public WaitCommand(IPlayer player, float timeS, CommandQueue queue)
    {
        this.player = player;
        this.timeMs = (int)(timeS * 1000);
        this.commandQueue = queue;
        Application.quitting += OnApplicationQuit;
    }

    public void Execute()
    {
        Wait().Forget();
    }

    private async UniTaskVoid Wait()
    {
        try
        {
            await UniTask.Delay(timeMs, cancellationToken: _cts.Token);
            OnCommandFinished();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Wait operation was canceled.");
        }
    }

    private void OnCommandFinished()
    {
        commandQueue.CommandFinished();
    }

    private void OnApplicationQuit()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}