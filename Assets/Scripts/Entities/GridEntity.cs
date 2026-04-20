using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int rootPosition;

    [Header("Entity Size")]
    // Lista de espaços que esta entidade ocupa
    public List<Vector2Int> occupiedOffsets = new List<Vector2Int> { Vector2Int.zero };

    // Registra a entidade no GridManager
    public void OccupyTiles()
    {
        if (gridManager == null) return;

        foreach (Vector2Int offset in occupiedOffsets)
        {
            gridManager.RegisterEntity(rootPosition + offset, this);
        }
    }

    // Limpa os registros no GridManager
    public void ClearTiles()
    {
        if (gridManager == null) return;

        if (gridManager.CheckIfOccupantIsMe(rootPosition, this))
        {
            gridManager.UnregisterEntity(rootPosition);
        }
    }

    // Se o objeto for destruido, limpa os tiles
    private void OnDestroy()
    {
        ClearTiles();
    }
}