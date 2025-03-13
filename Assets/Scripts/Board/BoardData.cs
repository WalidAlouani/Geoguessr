using System;
using System.Collections.Generic;

[Serializable]
public class BoardData
{
    public int Id;
    public List<TileData> Tiles;

    public BoardData(int id)
    {
        Id = id;
        Tiles = new List<TileData>();
    }
}
