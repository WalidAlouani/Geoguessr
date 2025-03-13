using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tileHomePrefab;
    [SerializeField] private GameObject tileQuizPrefab;
    [SerializeField] private GameObject tileQuizFlagPrefab;

    private List<TileData> tiles;
    private float multiplier = 0.5f;

    public void Init(List<TileData> tileDatas)
    {
        tiles = tileDatas;
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            var tileData = tiles[i];
            var position = new Vector3(tileData.Position.X * multiplier, 0, tileData.Position.Y * multiplier);
            GameObject prefab = null;
            switch (tileData.Type)
            {
                case TileType.Base:
                    if (i == 0)
                        prefab = tileHomePrefab;
                    else
                        prefab = tilePrefab;
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
            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
}
