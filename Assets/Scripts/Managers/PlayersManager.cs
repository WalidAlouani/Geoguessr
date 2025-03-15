using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class PlayersManager : MonoBehaviour
{
    [SerializeField] private List<PlayerData> playersData;

    private BoardManager boardManager;
    private IPlayerFactory _playerFactory;
    private List<PlayerController> players;
    private int currentPlayerIndex = 0;

    [Inject]
    public void Construct(IPlayerFactory playerFactory)
    {
        _playerFactory = playerFactory;
    }

    public void Init(BoardManager boardManager)
    {
        this.boardManager = boardManager;
        players = new List<PlayerController>();

        var initialPosition = boardManager.GetHomeTilePosition();
        for (int i = 0; i < playersData.Count; i++)
        {
            var playerData = playersData[i];

            var player = _playerFactory.Create(playerData, initialPosition, i);

            // (Optionally, if BoardManager is not injected into PlayerController, set it here.)
            player.boardManager = boardManager;
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
