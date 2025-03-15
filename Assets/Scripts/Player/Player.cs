using System;
using Zenject;

public class Player
{
    public int Index { get; private set; }
    public PlayerController Controller { get; set; }
    public string Name { get; private set; }
    public PlayerType Type { get; private set; }
    public int Coins { get; private set; }

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public Player(int index, string name, PlayerType type, int coins, SignalBus signalBus)
    {
        Index = index;
        Name = name;
        Type = type;
        Coins = coins;
        _signalBus = signalBus;
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
