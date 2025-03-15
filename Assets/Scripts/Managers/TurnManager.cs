using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour
{
    private PlayersManager playersManager;
    private BoardManager boardManager;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Init(PlayersManager playersManager, BoardManager boardManager)
    {
        this.playersManager = playersManager;
        this.boardManager = boardManager;

        SubscribeToCurrentPlayer();
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

        playersManager.Current.MoveSteps(steps);
    }

    private void SubscribeToCurrentPlayer()
    {
        playersManager.Current.OnMoveComplete += OnPlayerMoveComplete;
    }

    private void UnsubscribeFromCurrentPlayer()
    {
        playersManager.Current.OnMoveComplete -= OnPlayerMoveComplete;
    }

    private void OnPlayerMoveComplete()
    {
        UnsubscribeFromCurrentPlayer();

        playersManager.NextPlayer();

        SubscribeToCurrentPlayer();

        playersManager.Current.TurnStarted();
    }
}
