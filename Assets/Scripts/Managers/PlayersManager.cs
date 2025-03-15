using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] private List<PlayerData> playersData;

    private IPlayerFactory _playerFactory;
    private SignalBus _signalBus;

    private List<Player> players;
    private int currentPlayerIndex = 0;

    public Player Current => players[currentPlayerIndex];

    [Inject]
    public void Construct(SignalBus signalBus, IPlayerFactory playerFactory)
    {
        _playerFactory = playerFactory;
        _signalBus = signalBus;
    }

    public void Init(Vector3 spawnPosition)
    {
        players = new List<Player>();

        for (int i = 0; i < playersData.Count; i++)
        {
            var playerData = playersData[i];
            var player = new Player(i, playerData.Name, playerData.Type, 0, _signalBus);
            player.Controller = _playerFactory.Create(player, spawnPosition);
            players.Add(player);
            _signalBus.Fire(new PlayerCreatedSignal(player));
        }
    }

    public void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }
}
