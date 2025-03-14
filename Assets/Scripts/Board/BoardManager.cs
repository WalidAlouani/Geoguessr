using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private TileItem tilePrefab;
    [SerializeField] private TileItem tileHomePrefab;
    [SerializeField] private TileItem tileQuizPrefab;
    [SerializeField] private TileItem tileQuizFlagPrefab;

    private List<TileData> tileDatas;
    private List<TileItem> tileItems;
    private float multiplier = 0.5f;
    public int TilesCount { get; private set; }

    public void Init(List<TileData> tileDatas)
    {
        this.tileDatas = tileDatas;
        TilesCount = tileDatas.Count;
        CreateBoard();
    }

    private void CreateBoard()
    {
        tileItems = new List<TileItem>();
        for (int i = 0; i < tileDatas.Count; i++)
        {
            var tileData = tileDatas[i];
            var position = new Vector3(tileData.Position.X * multiplier, 0, tileData.Position.Y * multiplier);
            TileItem prefab = null;
            switch (tileData.Type)
            {
                case TileType.Base:
                    prefab = tilePrefab;
                    break;
                case TileType.Home:
                    prefab = tileHomePrefab;
                    break;
                case TileType.Quiz:
                    prefab = tileQuizPrefab;
                    break;
                case TileType.QuizFlag:
                    prefab = tileQuizFlagPrefab;
                    break;
                default:
                    prefab = tilePrefab;
                    break;
            }
            var tileItem = Instantiate(prefab, position, Quaternion.identity, transform);
            tileItem.Init(i);
            tileItems.Add(tileItem);
        }
    }

    public TileItem GetTile(int nextTileIndex)
    {
        return tileItems[nextTileIndex % TilesCount];
    }

    public TileItem GetHomeTile()
    {
        return GetTile(0);
    }
}
