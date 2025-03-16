using UnityEngine;
using System;
using System.Collections;
using Zenject;
using System.Collections.Generic;

public enum PlayerState { Idle, Moving }

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerVisual visual;
    [SerializeField] private PlayerAnimator _animation;
    [SerializeField] private float moveSpeed = 2f; // Speed for animation

    public int Index { get; private set; }
    public int CurrentTileIndex { get; private set; }


    public event Action<PlayerState> OnStateChange;
    public event Action<int> OnTileReached;
    public event Action OnMoveComplete;

    protected Player _player;
    protected PlayerState state;
    protected SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Init(Player player)
    {
        _player = player;
        Index = player.Index;
        visual.Init(Index);
        _animation.Init();
        SetState(PlayerState.Idle);
    }

    public void MoveSteps(List<Vector3> steps)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToTile(steps));
    }

    private IEnumerator MoveToTile(List<Vector3> steps)
    {
        SetState(PlayerState.Moving);
        _animation.PlayLoopingRotation(false);

        for (int i = 0; i < steps.Count; i++)
        {
            
            Vector3 startPos = transform.position;
            Vector3 endPos = steps[i];
            float journey = 0f;
            while (journey < 1f)
            {
                journey += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, journey);
                _animation.UpdateMovingTime(journey);
                yield return null;
            }

            CurrentTileIndex++;

            _signalBus.Fire(new TileReachedSignal(_player, CurrentTileIndex));
        }

        SetState(PlayerState.Idle);
        _animation.PlayLoopingRotation(true);

        _signalBus.Fire(new TileStoppedSignal(_player, CurrentTileIndex));

        OnMoveComplete?.Invoke();
    }

    public virtual void TurnStarted()
    {
        Debug.Log("Now it is player " + Index + "'s turn.");
    }

    public void Teleport(TileItem toTile)
    {
        transform.position = toTile.transform.position;
        CurrentTileIndex = toTile.Index;
        OnTileReached?.Invoke(CurrentTileIndex);
    }

    private void SetState(PlayerState state)
    {
        if (this.state == state)
            return;

        this.state = state;
        OnStateChange?.Invoke(state);
    }

}
