using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardDataSO boardData;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private CameraBoundaries cameraBoundaries;

    public event Action OnTurnPlayed;

    private void Start()
    {
        boardManager.Init(boardData.GetTiles());
        cameraBoundaries.Init(boardData.GetBoardCenter());
    }


    public int PlayTurn()
    {
        var number = UnityEngine.Random.Range(0, 10);
        OnTurnPlayed?.Invoke();
        return number;
    }


}
