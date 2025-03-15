using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour
{
    private PlayersManager playersManager;
    private BoardManager boardManager;

    private SignalBus _signalBus;
    private CommandQueue _commandQueue;

    [Inject]
    public void Construct(SignalBus signalBus, CommandQueue commandQueue)
    {
        _signalBus = signalBus;
        _commandQueue = commandQueue;
    }

    public void Init(PlayersManager playersManager, BoardManager boardManager)
    {
        this.playersManager = playersManager;
        this.boardManager = boardManager;

        SubscribeToCurrentPlayer();
        playersManager.Current.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(playersManager.Current));
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<DiceRolledSignal>(OnDiceRolled);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<DiceRolledSignal>(OnDiceRolled);
    }

    public void OnDiceRolled(DiceRolledSignal signal)
    {
        var steps = signal.Steps;
        Debug.Log("Player " + playersManager.Current.Index + " rolls: " + steps);

        var currentTileIndex = playersManager.Current.Controller.CurrentTileIndex;
        var tiles = boardManager.GetTiles(currentTileIndex, steps);

        ICommand moveCommand = new MoveCommand(playersManager.Current, tiles, _commandQueue);
        _commandQueue.EnqueueCommand(moveCommand);
    }

    private void SubscribeToCurrentPlayer()
    {
        playersManager.Current.Controller.OnMoveComplete += OnPlayerMoveComplete;
    }

    private void UnsubscribeFromCurrentPlayer()
    {
        playersManager.Current.Controller.OnMoveComplete -= OnPlayerMoveComplete;
    }

    private void OnPlayerMoveComplete()
    {
        UnsubscribeFromCurrentPlayer();
        playersManager.Current.TurnEnded();
        _signalBus.Fire(new TurnEndedSignal(playersManager.Current));

        playersManager.NextPlayer();

        SubscribeToCurrentPlayer();
        playersManager.Current.TurnStarted();
        _signalBus.Fire(new TurnStartedSignal(playersManager.Current));
    }
}
