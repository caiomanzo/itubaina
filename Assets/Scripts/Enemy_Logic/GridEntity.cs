using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int rootPosition;

    [Header("Entity Size")]
    public List<Vector2Int> occupiedOffsets = new List<Vector2Int> { Vector2Int.zero };

    void Start()
    {
        rootPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        OccupyTiles();
    }

    // Entidade registra quais tiles ela ocupa
    public void OccupyTiles()
    {
        foreach (Vector2Int offset in occupiedOffsets)
        {
            Tile tile = gridManager.GetTileAt(rootPosition + offset);
            if (tile != null) tile.occupant = this;
        }
    }

    // Limpa os tiles que desocupar
    public void ClearTiles()
    {
        foreach (Vector2Int offset in occupiedOffsets)
        {
            Tile tile = gridManager.GetTileAt(rootPosition + offset);
            if (tile != null && tile.occupant == this)
            {
                tile.occupant = null;
            }
        }
    }
}