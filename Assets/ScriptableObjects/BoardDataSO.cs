using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/BoardData", order = 2)]
public class BoardDataSO : ScriptableObject
{
    [SerializeField] private List<TileData> Tiles;
    [SerializeField] private Vector2 BoardCenter;

    public void SetTiles(List<TileData> tiles)
    {
        Tiles = tiles;

        if (tiles.Count == 0)
            return;

        var referencePosition = tiles[0].Position;

        for (int i = 0; i < tiles.Count; i++)
        {
            var tile = tiles[i];
            tile.Position = new Coordinates(referencePosition.X - tile.Position.X, tile.Position.Y - referencePosition.Y);
        }

        tiles.RemoveAt(tiles.Count - 1);

        BoardCenter = BoardUtilities.GetBoardCenterFromExtremeTiles(tiles.Select(el => new Vector2(el.Position.X, el.Position.Y)).ToList());
    }

    public List<TileData> GetTiles() => Tiles;
    public Vector2 GetBoardCenter() => BoardCenter;
}

public static class BoardUtilities
{
    /// <summary>
    /// Calculates the center of a board based on the positions of its tiles,
    /// using the tile with min X, min Y, max X, and max Y.
    /// </summary>
    public static Vector2 GetBoardCenterFromExtremeTiles(List<Vector2> tilePositions)
    {
        if (tilePositions == null || tilePositions.Count == 0)
            return Vector2.zero;

        float minX = tilePositions[0].x;
        float maxX = tilePositions[0].x;
        float minY = tilePositions[0].y;
        float maxY = tilePositions[0].y;

        foreach (Vector2 pos in tilePositions)
        {
            if (pos.x < minX)
                minX = pos.x;
            if (pos.x > maxX)
                maxX = pos.x;
            if (pos.y < minY)
                minY = pos.y;
            if (pos.y > maxY)
                maxY = pos.y;
        }

        // The center is the midpoint between the extreme positions.
        return new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
    }
}

