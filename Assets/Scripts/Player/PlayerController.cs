using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public BoardManager boardManager;
    [SerializeField] private PlayerVisual visual;
    [SerializeField] private float moveSpeed = 2f; // Speed for animation

    public int Index { get; private set; }
    public string Name { get; private set; }
    public bool isAI { get; private set; }

    private int currentTileIndex = 0;

    // Event that is invoked when the move is complete.
    public event Action<int> OnTileReached;
    public event Action OnMoveComplete;

    public void Init(int index, PlayerData playerData)
    {
        Index = index;
        Name = playerData.Name;
        isAI = playerData.Type == PlayerType.AI;
        visual.SetVisual(index);
    }

    // Called to move the player a given number of steps.
    public void MoveSteps(int steps)
    {
        int targetTileIndex = currentTileIndex + steps;

        StopAllCoroutines();
        StartCoroutine(MoveToTile(targetTileIndex));
    }

    // Coroutine to animate movement from tile to tile.
    private IEnumerator MoveToTile(int targetTileIndex)
    {
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
                yield return null;
            }

            OnTileReached?.Invoke(currentTileIndex);

            nextTile.OnTileReached();// remove

            currentTileIndex++;
        }
        // Signal that movement is complete.
        OnMoveComplete?.Invoke();
    }
}
