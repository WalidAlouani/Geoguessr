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
        var tiles = boardManager.GetTiles(playersManager.Current.Index, signal.Steps);
        playersManager.Current.Move(tiles);
    }

    private void SubscribeToCurrentPlayer()
    {
        _signalBus.Subscribe<PlayerFinishMoveSignal>(OnPlayerMoveComplete);
    }

    private void UnsubscribeFromCurrentPlayer()
    {
        _signalBus.Unsubscribe<PlayerFinishMoveSignal>(OnPlayerMoveComplete);
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
