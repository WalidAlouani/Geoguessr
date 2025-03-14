using UnityEngine;
using System;
using System.Collections;

public class PlayerControllerAI : PlayerController
{
    public override void TurnStarted()
    {
        StartCoroutine(AutoMove());
    }

    private IEnumerator AutoMove()
    {
        yield return new WaitForSeconds(2f); // Delay to simulate AI
        GameEvents.RequestDiceRoll();
    }
}
