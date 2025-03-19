using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurnManager : IInitializable, IDisposable
{
    private PlayersManager _playersManager;
    private BoardManager _boardManager;

    private SignalBus _signalBus;
    private CommandQueue _commandQueue;
    private ITurnCommandProcessor _commandProcessor;
    private int _currentPlayerIndex = 0;
    public IPlayer CurrentPlayer => _playersManager.Players[_currentPlayerIndex];

    [Inject]
    public TurnManager(SignalBus signalBus, CommandQueue commandQueue, ITurnCommandProcessor commandProcessor)
    {
        _signalBus = signalBus;
        _commandQueue = commandQueue;
        _commandProcessor = commandProcessor;
    }

    public void Init(PlayersManager playersManager, BoardManager boardManager)
    {
        this._playersManager = playersManager;
        this._boardManager = boardManager;

        CurrentPlayer.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(CurrentPlayer));
    }


    public void Initialize()
    {
        _signalBus.Subscribe<DiceRolledSignal>(OnDiceRolled);
        _signalBus.Subscribe<QuizRequestedSignal>(OnQuizRequested);
        _signalBus.Subscribe<QuizFinishedSignal>(OnQuizFinished);
        _commandQueue.OnQueueEmpty += OnChangeTurn;
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<DiceRolledSignal>(OnDiceRolled);
        _signalBus.Unsubscribe<QuizRequestedSignal>(OnQuizRequested);
        _signalBus.Unsubscribe<QuizFinishedSignal>(OnQuizFinished);
        _commandQueue.OnQueueEmpty -= OnChangeTurn;
    }

    private void OnDiceRolled(DiceRolledSignal signal)
    {
        List<Vector3> tiles = _boardManager.GetTilesForPlayerMovement(CurrentPlayer.Index, signal.Steps);
        _commandProcessor.ProcessDiceRoll(CurrentPlayer, tiles);
    }

    private void OnQuizRequested(QuizRequestedSignal signal)
    {
        _commandProcessor.ProcessQuizRequest(CurrentPlayer, signal.QuizType, signal.SceneName);
    }

    private void OnQuizFinished(QuizFinishedSignal signal)
    {
        _commandProcessor.ProcessQuizFinished();

        CurrentPlayer.AddCoins(signal.CoinAmount);
        var position = _boardManager.GetPlayerPosition(CurrentPlayer.Index);
        _signalBus.Fire(new CoinsAddedSignal(CurrentPlayer, position, signal.CoinAmount));
    }

    private void OnChangeTurn()
    {
        CurrentPlayer.TurnEnded();
        _signalBus.Fire(new TurnEndedSignal(CurrentPlayer));

        _currentPlayerIndex = (_currentPlayerIndex + 1) % _playersManager.Players.Count;

        CurrentPlayer.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(CurrentPlayer));
    }
}
