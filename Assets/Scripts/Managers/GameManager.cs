using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayersManager playersManager;
    [SerializeField] private BoardDataSO boardData;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private CameraBoundaries cameraBoundaries;


    private void Start()
    {
        boardManager.Init(boardData.GetTiles());
        cameraBoundaries.Init(boardData.GetBoardCenter());
        playersManager.Init(boardManager);
        turnManager.Init(playersManager);
    }

}
