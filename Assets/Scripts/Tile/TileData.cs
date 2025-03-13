using System;

[Serializable]
public class TileData
{
    public Coordinates Position;
    public TileType Type;
}

[Serializable]
public struct Coordinates
{
    public int X;
    public int Y;

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
}
