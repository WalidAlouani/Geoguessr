using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player
{
    public int Index { get; private set; }
    public PlayerController Controller { get; set; }
    public string Name { get; private set; }
    public PlayerType Type { get; private set; }
    public int Coins { get; private set; }

    private SignalBus _signalBus;
    private CommandQueue _commandQueue;

    public Player(int index, string name, PlayerType type, int coins, SignalBus signalBus)
    {
        Index = index;
        Name = name;
        Type = type;
        Coins = coins;
        _signalBus = signalBus;
        _commandQueue = new CommandQueue();
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

    public void Move(List<Vector3> tiles)
    {
        _signalBus.Fire(new PlayerStartMoveSignal(this));

        ICommand waitCommand = new WaitCommand(this, 1, _commandQueue);
        _commandQueue.EnqueueCommand(waitCommand);

        ICommand moveCommand = new MoveCommand(this, tiles, _commandQueue);
        _commandQueue.EnqueueCommand(moveCommand);

        waitCommand = new WaitCommand(this, 1, _commandQueue, ()=> _signalBus.Fire(new PlayerFinishMoveSignal(this))); 
        _commandQueue.EnqueueCommand(waitCommand);
    }
}
