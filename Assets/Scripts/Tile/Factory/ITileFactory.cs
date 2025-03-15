using UnityEngine;

public interface ITileFactory
{
    TileItem CreateTile(TileData tileData, Vector3 position, Transform parent);
}
