using System;
using Zenject;

public class Player
{
    public int Index { get; }
    public string Name { get;}
    public PlayerType Type { get;}
    public int Coins { get; private set; }
    public PlayerController Controller { get; private set; }

    private readonly SignalBus _signalBus;

    public Player(int index, string name, PlayerType type, int coins, SignalBus signalBus)
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

    public void TurnStarted()
    {
        Controller.TurnStarted();
    }

    public void TurnEnded()
    {
    }

    public void AddCoins(int coinAmount)
    {
        Coins += coinAmount;
        _signalBus.Fire(new CoinsUpdateSignal(this));
    }
}
