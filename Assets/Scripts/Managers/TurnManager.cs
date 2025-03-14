using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public List<PlayerController> players;

    private int currentPlayerIndex = 0;

    public event Action OnTurnPlayed;

    void Start()
    {
        if (players == null || players.Count == 0)
        {
            Debug.LogError("No players assigned to TurnManager!");
            return;
        }
        SubscribeToCurrentPlayer();
    }

    // Called by the UI button.
    public int RollDice()
    {
        // Generate a random number between 0 and 10 (inclusive).
        int randomSteps = UnityEngine.Random.Range(1, 11);
        Debug.Log("Player " + currentPlayerIndex + " rolls: " + randomSteps);
        players[currentPlayerIndex].MoveSteps(randomSteps);
        OnTurnPlayed?.Invoke();
        return randomSteps;
    }

    // Subscribe to the current player's move-completion event.
    private void SubscribeToCurrentPlayer()
    {
        players[currentPlayerIndex].OnMoveComplete += OnPlayerMoveComplete;
    }

    // Unsubscribe from the current player's event to avoid duplicate subscriptions.
    private void UnsubscribeFromCurrentPlayer()
    {
        players[currentPlayerIndex].OnMoveComplete -= OnPlayerMoveComplete;
    }

    // Called when the current player's move is complete.
    private void OnPlayerMoveComplete()
    {
        UnsubscribeFromCurrentPlayer();
        // Advance turn.
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        Debug.Log("Now it is player " + currentPlayerIndex + "'s turn.");
        SubscribeToCurrentPlayer();

        if (players[currentPlayerIndex].isAI)
        {
            StartCoroutine(AIAutoMove());
        }
    }

    private IEnumerator AIAutoMove()
    {
        yield return new WaitForSeconds(2f); // Delay to simulate AI
        RollDice();
    }
}
