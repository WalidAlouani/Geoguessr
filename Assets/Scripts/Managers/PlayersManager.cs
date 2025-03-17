using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayersManager : MonoBehaviour
{
    private IPlayerFactory _playerFactory;
    private SignalBus _signalBus;

    public List<Player> Players { get; private set; }

    [Inject]
    public void Construct(SignalBus signalBus, IPlayerFactory playerFactory)
    {
        _playerFactory = playerFactory;
        _signalBus = signalBus;
    }

    public void Init(List<PlayerData> playersData, Vector3 spawnPosition)
    {
        Players = new List<Player>();
        for (int i = 0; i < playersData.Count; i++)
        {
            var playerData = playersData[i];
            var player = new Player(i, playerData.Name, playerData.Type, 0, _signalBus);
            var playerController = _playerFactory.Create(player, spawnPosition);
            player.SetController(playerController);
            Players.Add(player);
        }
        _signalBus.Fire(new PlayersCreatedSignal(Players));
    }
}
