using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnTurnPlayed;
    public int PlayTurn()
    {
        var number = UnityEngine.Random.Range(0, 10);
        OnTurnPlayed?.Invoke();
        return number;
    }
}
