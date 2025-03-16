using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private List<TileItem> tileItems;
    private float multiplier = .5f;
    public int TilesCount { get; private set; }

    private SignalBus _signalBus;
    private ITileFactory _tileFactory;
    private List<int> _positions = new List<int>();

    [Inject]
    public void Construct(ITileFactory tileFactory, SignalBus signalBus)
    {
        _signalBus = signalBus;
        _tileFactory = tileFactory;
    }

    public void Init(List<TileData> tileDatas)
    {
        TilesCount = tileDatas.Count;
        CreateBoard(tileDatas);
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<TileReachedSignal>(OnTileReached);
        _signalBus.Subscribe<TileStoppedSignal>(OnTileStopped);
        _signalBus.Subscribe<PlayersCreatedSignal>(OnPlayersCreated);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<TileReachedSignal>(OnTileReached);
        _signalBus.Unsubscribe<TileStoppedSignal>(OnTileStopped);
        _signalBus.Unsubscribe<PlayersCreatedSignal>(OnPlayersCreated);
    }

    private void CreateBoard(List<TileData> tileDatas)
    {
        tileItems = new List<TileItem>();
        for (int i = 0; i < tileDatas.Count; i++)
        {
            var tileData = tileDatas[i];
            var position = new Vector3(tileData.Position.X * multiplier, 0, tileData.Position.Y * multiplier);

            var tileItem = _tileFactory.CreateTile(tileData, position, transform);
            tileItem.Init(i);
            tileItems.Add(tileItem);
        }
    }

    public TileItem GetTile(int tileIndex)
    {
        return tileItems[tileIndex % TilesCount];
    }

    public Vector3 GetTilePosition(int tileIndex)
    {
        return GetTile(tileIndex).transform.position;
    }

    public Vector3 GetHomeTilePosition()
    {
        return GetTilePosition(0);
    }

    public List<Vector3> GetTiles(int playerIndex, int steps)
    {
        var currentTileIndex = _positions[playerIndex];

        List<Vector3> tiles = new List<Vector3>();
        int targetTileIndex = currentTileIndex + steps;
        while (currentTileIndex < targetTileIndex)
        {
            currentTileIndex++;
            Vector3 nextTile = GetTilePosition(currentTileIndex);
            tiles.Add(nextTile);
        }

        return tiles;
    }

    private void OnTileReached(TileReachedSignal signal)
    {
        GetTile(signal.TileIndex).TriggerOnReachEvent(signal.Player, null);
    }

    private void OnTileStopped(TileStoppedSignal signal)
    {
        GetTile(signal.TileIndex).TriggerOnStopEvent(signal.Player, null);
        _positions[signal.Player.Index] = signal.TileIndex;
    }

    private void OnPlayersCreated(PlayersCreatedSignal signal)
    {
        for (int i = 0; i < signal.Players.Count; i++)
        {
            _positions.Add(0);
        }
    }
}
