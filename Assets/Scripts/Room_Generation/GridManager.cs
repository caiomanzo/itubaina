using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> map = new Dictionary<Vector2Int, Tile>();

    void Awake()
    {
        Tile[] everyTile = FindObjectsByType<Tile>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (Tile tile in everyTile)
        {
            tile.coordinate = new Vector2Int(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y));

            if (!map.ContainsKey(tile.coordinate))
            {
                map.Add(tile.coordinate, tile);
            }
        }
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        if (map.TryGetValue(pos, out Tile tile)) return tile;
        return null;
    }

    // Verifica se o tile está livre para andar
    public bool CheckWalkability(Vector2Int target, out GridEntity foundOccupant)
    {
        foundOccupant = null;

        if (map.TryGetValue(target, out Tile tile))
        {
            if (tile.type != TileType.Floor) return false;

            foundOccupant = tile.occupant;

            return foundOccupant == null;
        }

        return false;
    }
}