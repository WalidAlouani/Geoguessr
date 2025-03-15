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
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<TileReachedSignal>(OnTileReached);
        _signalBus.Unsubscribe<TileStoppedSignal>(OnTileStopped);
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

            switch (tileData.Type)
            {
                case TileType.Home:
                    tileItem.SetEvent(null, new CoinRewardEvent(10000));
                    break;
                case TileType.Base:
                    tileItem.SetEvent(new CoinRewardEvent(1000), null);
                    break;
                case TileType.Quiz:
                    //tileItem.SetEvent(new CoinRewardEvent(100), null);
                    break;
                case TileType.QuizFlag:
                    //tileItem.SetEvent(new CoinRewardEvent(100), null);
                    break;
            }

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

    public List<Vector3> GetTiles(int currentTileIndex, int steps)
    {
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
    }
}
