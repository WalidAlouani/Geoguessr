using UnityEngine;
using System;
using System.Collections;
using Zenject;

public class PlayerControllerAI : PlayerController
{
    public override void TurnStarted()
    {
        base.TurnStarted();
        StartCoroutine(AutoMove());
    }

    private IEnumerator AutoMove()
    {
        yield return new WaitForSeconds(2f); // Delay to simulate AI
        _signalBus.Fire<RollDiceSignal>();
    }
}
