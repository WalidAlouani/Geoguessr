using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayersManager : MonoBehaviour
{
    [SerializeField] private List<PlayerData> playersData;
    [SerializeField] private PlayerController playerPrefab;

    private BoardManager boardManager;
    private List<PlayerController> players;
    private int currentPlayerIndex = 0;

    public void Init(BoardManager boardManager)
    {
        this.boardManager = boardManager;
        players = new List<PlayerController>();

        var initialPosition = boardManager.GetHomeTile().transform.position;

        for (int i = 0; i < playersData.Count; i++)
        {
            var player = Instantiate(playerPrefab, initialPosition, Quaternion.identity);
            player.Init(i, playersData[i]);
            player.boardManager = boardManager; // hack remove later
            players.Add(player);
        }
    }

    public void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }

    public PlayerController Current => players[currentPlayerIndex];
}

public enum PlayerType { Humain, AI }
[Serializable]
public class PlayerData
{
    public string Name;
    public PlayerType Type;
}
