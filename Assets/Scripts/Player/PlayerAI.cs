using System.Threading;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

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
        await ThrowDice(_cts.Token);
    }

    public void TurnEnded()
    {
        //Debug.Log("Now it is player " + Index + "'s turn.");
    }

    public void AddCoins(int coinAmount)
    {
        Coins += coinAmount;
        _signalBus.Fire(new CoinsUpdateSignal(this));
    }

    public void CancelTurnOperation()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public void Dispose()
    {
        CancelTurnOperation();
    }

    public async UniTask ThrowDice(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(2000, cancellationToken: token);
            _signalBus.Fire(new RollDiceSignal(this));
        }
        catch (OperationCanceledException)
        {
            Debug.Log("ThrowDice operation was canceled.");
        }
    }
}
