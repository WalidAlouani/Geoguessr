using UnityEngine;
using Zenject;

public class CompositeTileFactory : ITileFactory
{
    private readonly DiContainer _container;
    private readonly TilePrefabMapping _tilePrefabMapping;

    public CompositeTileFactory(DiContainer container, TilePrefabMapping tilePrefabMapping)
    {
        _container = container;
        _tilePrefabMapping = tilePrefabMapping;
    }

    public TileItem CreateTile(TileData tileData, Vector3 position, Transform parent)
    {
        var entry = _tilePrefabMapping.entries.Find(e => e.tileType == tileData.Type);
        if (entry == null)
        {
            throw new System.Exception("No prefab mapping found for tile type: " + tileData.Type);
        }

        GameObject tileObj = _container.InstantiatePrefab(entry.tilePrefab.gameObject, position, Quaternion.identity, parent);
        TileItem tileItem = tileObj.GetComponent<TileItem>();
        if (tileItem == null)
        {
            throw new System.Exception("The prefab for " + tileData.Type + " does not have a TileItem component.");
        }
        return tileItem;
    }
}
