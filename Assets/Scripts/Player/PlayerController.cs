using UnityEngine;
using System;
using System.Collections;

public enum PlayerState { Idle, Moving }

public class PlayerController : MonoBehaviour
{
    public BoardManager boardManager;
    [SerializeField] private PlayerVisual visual;
    [SerializeField] private PlayerAnimator _animation;
    [SerializeField] private float moveSpeed = 2f; // Speed for animation

    public int Index { get; private set; }
    public string Name { get; private set; }

    public event Action<PlayerState> OnStateChange;
    public event Action<int> OnTileReached;
    public event Action OnMoveComplete;

    private PlayerState state;
    private int currentTileIndex = 0;

    public void Init(int index, PlayerData playerData)
    {
        Index = index;
        Name = playerData.Name;
        visual.Init(index);
        _animation.Init();
        SetState(PlayerState.Idle);
    }

    private void SetState(PlayerState state)
    {
        if (this.state == state)
            return;

        this.state = state;
        OnStateChange?.Invoke(state);
    }

    public void MoveSteps(int steps)
    {
        int targetTileIndex = currentTileIndex + steps;

        StopAllCoroutines();
        StartCoroutine(MoveToTile(targetTileIndex));
    }

    private IEnumerator MoveToTile(int targetTileIndex)
    {
        SetState(PlayerState.Moving);
        _animation.PlayLoopingRotation(false);

        while (currentTileIndex < targetTileIndex)
        {
            int nextTileIndex = currentTileIndex + 1;
            TileItem nextTile = boardManager.GetTile(nextTileIndex);
            if (nextTile == null) break;

            Vector3 startPos = transform.position;
            Vector3 endPos = nextTile.transform.position;
            float journey = 0f;
            while (journey < 1f)
            {
                journey += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, journey);
                _animation.UpdateMovingTime(journey);
                yield return null;
            }

            OnTileReached?.Invoke(currentTileIndex);

            nextTile.OnTileReached();// remove

            currentTileIndex++;
        }

        OnMoveComplete?.Invoke();
        SetState(PlayerState.Idle);
        _animation.PlayLoopingRotation(true);
    }

    public virtual void TurnStarted()
    {
        Debug.Log("Now it is player " + Index + "'s turn.");
    }
}
