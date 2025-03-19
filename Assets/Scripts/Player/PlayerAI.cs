using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerAI : IPlayer
{
    public int Index { get; }
    public string Name { get; }
    public PlayerType Type { get; }
    public int Coins { get; private set; }
    public PlayerController Controller { get; private set; }

    private readonly SignalBus _signalBus;
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public PlayerAI(PlayerType type, int index, string name, int coins, SignalBus signalBus)
    {
        Index = index;
        Name = name;
        Type = type;
        Coins = coins;
        _signalBus = signalBus;
    }

    public void SetController(PlayerController controller)
    {
        Controller = controller;
    }

    public async void TurnStarted()
    {
        Debug.Log("Now it is player " + Index + "'s turn.");
        try
        {
            await Task.Delay(2000, _cts.Token);
            _signalBus.Fire(new RollDiceSignal(this));
        }
        catch (OperationCanceledException)
        {
            Debug.Log("TurnStarted operation was canceled.");
        }
    }

    public void TurnEnded()
    {
    }

    public void AddCoins(int coinAmount)
    {
        Coins += coinAmount;
        _signalBus.Fire(new CoinsUpdateSignal(this));
    }

    public void CancelTurnOperation()
    {
        _cts.Cancel();
    }

    public void Dispose()
    {
        CancelTurnOperation();
    }
}
