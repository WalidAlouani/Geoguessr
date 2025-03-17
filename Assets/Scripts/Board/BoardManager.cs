using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private List<TileItem> _tileItems;
    private int _tilesCount;
    private float _multiplier = .5f;
    private List<int> _playerTilePositions = new List<int>();
    private ITileFactory _tileFactory;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(ITileFactory tileFactory, SignalBus signalBus)
    {
        _signalBus = signalBus;
        _tileFactory = tileFactory;
    }

    public void Init(List<TileData> tileDatas)
    {
        _tilesCount = tileDatas.Count;
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
        _tileItems = new List<TileItem>();
        for (int i = 0; i < tileDatas.Count; i++)
        {
            var tileData = tileDatas[i];
            var position = new Vector3(tileData.Position.X * _multiplier, 0, tileData.Position.Y * _multiplier);

            var tileItem = _tileFactory.CreateTile(tileData, position, transform);
            tileItem.Init(i);
            _tileItems.Add(tileItem);
        }
    }

    public TileItem GetTile(int tileIndex)
    {
        return _tileItems[tileIndex % _tilesCount];
    }

    public Vector3 GetTilePosition(int tileIndex)
    {
        return GetTile(tileIndex).transform.position;
    }

    public Vector3 GetHomeTilePosition()
    {
        return GetTilePosition(0);
    }

    public List<Vector3> GetTilesForPlayerMovement(int playerIndex, int steps)
    {
        int currentTileIndex = _playerTilePositions[playerIndex];
        List<Vector3> path = new List<Vector3>();
        int targetTileIndex = currentTileIndex + steps;

        for (int i = currentTileIndex + 1; i <= targetTileIndex; i++)
        {
            path.Add(GetTilePosition(i));
        }
        return path;
    }

    private void OnTileReached(TileReachedSignal signal)
    {
        GetTile(signal.TileIndex).TriggerOnReachEvent(signal.Player);
    }

    private void OnTileStopped(TileStoppedSignal signal)
    {
        GetTile(signal.TileIndex).TriggerOnStopEvent(signal.Player);
        _playerTilePositions[signal.Player.Index] = signal.TileIndex;
    }

    private void OnPlayersCreated(PlayersCreatedSignal signal)
    {
        for (int i = 0; i < signal.Players.Count; i++)
        {
            _playerTilePositions.Add(0);
        }
    }
}
