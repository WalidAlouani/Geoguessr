using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    private PlayersManager playersManager;

    public event Action OnTurnPlayed;

    public void Init(PlayersManager playersManager)
    {
        this.playersManager = playersManager;

        SubscribeToCurrentPlayer();
    }

    // Called by the UI button.
    public int RollDice()
    {
        // Generate a random number between 0 and 10 (inclusive).
        int randomSteps = UnityEngine.Random.Range(1, 11);
        Debug.Log("Player " + playersManager.Current.Index + " rolls: " + randomSteps);
        playersManager.Current.MoveSteps(randomSteps);
        OnTurnPlayed?.Invoke();
        return randomSteps;
    }

    // Subscribe to the current player's move-completion event.
    private void SubscribeToCurrentPlayer()
    {
        playersManager.Current.OnMoveComplete += OnPlayerMoveComplete;
    }

    // Unsubscribe from the current player's event to avoid duplicate subscriptions.
    private void UnsubscribeFromCurrentPlayer()
    {
        playersManager.Current.OnMoveComplete -= OnPlayerMoveComplete;
    }

    // Called when the current player's move is complete.
    private void OnPlayerMoveComplete()
    {
        UnsubscribeFromCurrentPlayer();
        // Advance turn.
        playersManager.NextPlayer();
        Debug.Log("Now it is player " + playersManager.Current.Index + "'s turn.");
        SubscribeToCurrentPlayer();

        if (playersManager.Current.isAI)
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
