using System;
using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour
{
    private PlayersManager playersManager;
    private BoardManager boardManager;

    private SignalBus _signalBus;
    private CommandQueue _commandQueue = new CommandQueue();

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Init(PlayersManager playersManager, BoardManager boardManager)
    {
        this.playersManager = playersManager;
        this.boardManager = boardManager;
        
        playersManager.Current.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(playersManager.Current));
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<DiceRolledSignal>(OnDiceRolled);
        _signalBus.Subscribe<QuizRequestedSignal>(OnQuizRequested);
        _signalBus.Subscribe<QuizFinishedSignal>(OnQuizFinished);
        _commandQueue.OnQueueEmpty += OnChangeTurn;
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<DiceRolledSignal>(OnDiceRolled);
        _signalBus.Unsubscribe<QuizRequestedSignal>(OnQuizRequested);
        _signalBus.Unsubscribe<QuizFinishedSignal>(OnQuizFinished);
        _commandQueue.OnQueueEmpty -= OnChangeTurn;
    }

    public void OnDiceRolled(DiceRolledSignal signal)
    {
        var player = playersManager.Current;
        var tiles = boardManager.GetTiles(player.Index, signal.Steps);
        _signalBus.Fire(new PlayerStartMoveSignal(player));

        var waitCommand = new WaitCommand(player, 1, _commandQueue);
        _commandQueue.EnqueueCommand(waitCommand);

        var moveCommand = new MoveCommand(player, tiles, _commandQueue);
        _commandQueue.EnqueueCommand(moveCommand);

        waitCommand = new WaitCommand(player, 1, _commandQueue);
        _commandQueue.EnqueueCommand(waitCommand);
    }

    private void OnQuizRequested(QuizRequestedSignal signal)
    {
        var player = playersManager.Current;

        var quizCommand = new QuizCommand(signal.QuizType);
        _commandQueue.EnqueueCommand(quizCommand);

        var waitCommand = new WaitCommand(player, 1, _commandQueue);
        _commandQueue.EnqueueCommand(waitCommand);
    }

    private void OnQuizFinished()
    {
        _commandQueue.CommandFinished();
    }

    private void OnChangeTurn()
    {
        playersManager.Current.TurnEnded();
        _signalBus.Fire(new TurnEndedSignal(playersManager.Current));

        playersManager.NextPlayer();

        playersManager.Current.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(playersManager.Current));
    }
}
