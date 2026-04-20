using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Configuração das Camadas")]
    public LayerMask obstacleLayer;

    private Dictionary<Vector2Int, GridEntity> entitiesOnGrid = new Dictionary<Vector2Int, GridEntity>();
    public bool CheckWalkability(Vector2Int targetPosition, out GridEntity occupant)
    {
        occupant = null;

        // Checa Parede
        Vector2 checkPos = new Vector2(targetPosition.x, targetPosition.y);
        Collider2D hit = Physics2D.OverlapPoint(checkPos, obstacleLayer);

        if (hit != null) return false; // Parede

        // Checa Entidade
        if (entitiesOnGrid.TryGetValue(targetPosition, out occupant))
        {
            return false; // Espaco ocupado
        }

        return true; // Espaco livre
    }

    // Métodos para as Entidades se registrarem
    public void RegisterEntity(Vector2Int pos, GridEntity entity) => entitiesOnGrid[pos] = entity;
    public void UnregisterEntity(Vector2Int pos) => entitiesOnGrid.Remove(pos);
    public void ClearAllEntities() => entitiesOnGrid.Clear();

    public bool CheckIfOccupantIsMe(Vector2Int pos, GridEntity me)
    {
        if (entitiesOnGrid.TryGetValue(pos, out GridEntity occupant))
        {
            return occupant == me;
        }
        return false;
    }
}