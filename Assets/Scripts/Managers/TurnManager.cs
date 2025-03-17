using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour
{
    private PlayersManager playersManager;
    private BoardManager boardManager;

    private SignalBus _signalBus;
    private CommandQueue _commandQueue = new CommandQueue();

    private int _currentPlayerIndex = 0;
    public Player CurrentPlayer => playersManager.Players[_currentPlayerIndex];

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Init(PlayersManager playersManager, BoardManager boardManager)
    {
        this.playersManager = playersManager;
        this.boardManager = boardManager;

        CurrentPlayer.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(CurrentPlayer));
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
        var tiles = boardManager.GetTilesForPlayerMovement(CurrentPlayer.Index, signal.Steps);
        _signalBus.Fire(new PlayerStartMoveSignal(CurrentPlayer));

        var waitCommand = new WaitCommand(CurrentPlayer, 1, _commandQueue);
        _commandQueue.EnqueueCommand(waitCommand);

        var moveCommand = new MoveCommand(CurrentPlayer, tiles, _commandQueue);
        _commandQueue.EnqueueCommand(moveCommand);

        _commandQueue.EnqueueCommand(waitCommand);
    }

    private void OnQuizRequested(QuizRequestedSignal signal)
    {
        var quizCommand = new QuizCommand(signal.QuizType, signal.SceneName);
        _commandQueue.EnqueueCommand(quizCommand);

        var waitCommand = new WaitCommand(CurrentPlayer, 1, _commandQueue);
        _commandQueue.EnqueueCommand(waitCommand);
    }

    private void OnQuizFinished(QuizFinishedSignal signal)
    {
        _commandQueue.CommandFinished();

        CurrentPlayer.AddCoins(signal.CoinAmount);
        var position = boardManager.GetPlayerPosition(CurrentPlayer.Index);
        _signalBus.Fire(new CoinsAddedSignal(CurrentPlayer, position, signal.CoinAmount));
    }

    private void OnChangeTurn()
    {
        CurrentPlayer.TurnEnded();
        _signalBus.Fire(new TurnEndedSignal(CurrentPlayer));

        _currentPlayerIndex = (_currentPlayerIndex + 1) % playersManager.Players.Count;

        CurrentPlayer.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(CurrentPlayer));
    }
}
