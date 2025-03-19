using UnityEngine;
using Zenject;

public class PlayerHumain : IPlayer
{
    public int Index { get; }
    public string Name { get;}
    public PlayerType Type { get;}
    public int Coins { get; private set; }
    public PlayerController Controller { get; private set; }

    private readonly SignalBus _signalBus;

    public PlayerHumain(PlayerType type, int index, string name, int coins, SignalBus signalBus)
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
        //Debug.Log("Now it is player " + Index + "'s turn.");
    }

    public void TurnEnded()
    {
    }

    public void AddCoins(int coinAmount)
    {
        Coins += coinAmount;
        _signalBus.Fire(new CoinsUpdateSignal(this));
    }

    public void Dispose()
    {
    }
}
