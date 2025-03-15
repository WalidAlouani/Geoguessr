using System;
using UnityEngine;
using Zenject;

public class CompositePlayerFactory : IPlayerFactory
{
    private readonly DiContainer _container;
    private readonly PlayerPrefabMapping _playerPrefabMapping;

    public CompositePlayerFactory(DiContainer container, PlayerPrefabMapping playerPrefabMapping)
    {
        _container = container;
        _playerPrefabMapping = playerPrefabMapping;
    }

    public PlayerController Create(PlayerData playerData, Vector3 initialPosition, int index)
    {
        var entry = _playerPrefabMapping.entries.Find(e => e.playerType == playerData.Type);
        if (entry == null)
        {
            throw new Exception("No prefab found for player type: " + playerData.Type);
        }

        GameObject playerObj = _container.InstantiatePrefab(entry.prefab, initialPosition, Quaternion.identity, null);
        var player = playerObj.GetComponent<PlayerController>();
        if (player == null)
        {
            throw new Exception("The prefab for " + playerData.Type + " does not have a PlayerController component.");
        }

        player.Init(index, playerData);
        return player;
    }
}
