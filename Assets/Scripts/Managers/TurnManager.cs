using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    private PlayersManager playersManager;

    public void Init(PlayersManager playersManager)
    {
        this.playersManager = playersManager;

        SubscribeToCurrentPlayer();
    }

    private void OnEnable()
    {
        GameEvents.OnDiceRolled += OnDiceRolled;
    }

    private void OnDisable()
    {
        GameEvents.OnDiceRolled -= OnDiceRolled;
    }

    public void OnDiceRolled(int steps)
    {
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

        Debug.Log("Now it is player " + playersManager.Current.Index + "'s turn.");

        SubscribeToCurrentPlayer();

        playersManager.Current.TurnStarted();
    }
}
