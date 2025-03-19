using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayersManager : IDisposable
{
    private readonly IPlayerFactory _playerFactory;
    private readonly SignalBus _signalBus;

    public List<IPlayer> Players { get; private set; }

    [Inject]
    public PlayersManager(SignalBus signalBus, IPlayerFactory playerFactory)
    {
        _signalBus = signalBus;
        _playerFactory = playerFactory;
    }

    public void Init(List<PlayerData> playersData, Vector3 spawnPosition)
    {
        Players = new List<IPlayer>();

        for (int i = 0; i < playersData.Count; i++)
        {
            var playerData = playersData[i];
            var player = PlayerFactory.Create(playerData.Type, i, playerData.Name,  0, _signalBus);
            var playerController = _playerFactory.Create(player, spawnPosition);
            player.SetController(playerController);
            Players.Add(player);
        }
        _signalBus.Fire(new PlayersCreatedSignal(Players));
    }

    public void Dispose()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Dispose();
        }

        Players.Clear();
    }
}
