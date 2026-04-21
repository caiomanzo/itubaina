using UnityEngine;

[RequireComponent(typeof(GridEntity))]
[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    protected GridEntity myEntity;
    protected EntityManager localEntityManager;
    protected Player targetPlayer;
    protected Health myHealth;

    protected virtual void Start()
    {
        myEntity = GetComponent<GridEntity>();
        myHealth = GetComponent<Health>();

        myEntity.gridManager = GetComponentInParent<GridManager>();
        localEntityManager = GetComponentInParent<EntityManager>();

        if (myEntity.gridManager != null)
        {
            targetPlayer = myEntity.gridManager.GetComponentInChildren<Player>();
        }

        if (localEntityManager != null)
        {
            localEntityManager.RegisterEnemy(this);
        }

        myEntity.rootPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        myEntity.OccupyTiles();
    }

    public bool IsDead()
    {
        // Se não tiver o componente de vida, assume que não está morto dessa forma
        if (myHealth == null) return false;

        return myHealth.GetCurrentHealth() <= 0;
    }

    protected virtual void OnDestroy()
    {
        if (localEntityManager != null)
        {
            localEntityManager.UnregisterEnemy(this);
        }
    }

    public abstract void TakeTurn();

    protected int GetGridDistanceToPlayer()
    {
        if (targetPlayer == null) return 999;
        Vector2Int playerPos = targetPlayer.GetComponent<GridEntity>().rootPosition;
        Vector2Int myPos = myEntity.rootPosition;
        return Mathf.Abs(playerPos.x - myPos.x) + Mathf.Abs(playerPos.y - myPos.y);
    }

    protected bool TryMoveTo(Vector2Int targetPosition)
    {
        if (myEntity.gridManager == null) return false;

        if (myEntity.gridManager.CheckWalkability(targetPosition, out GridEntity occupant))
        {
            myEntity.ClearTiles();
            myEntity.rootPosition = targetPosition;
            myEntity.OccupyTiles();
            transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
            return true;
        }
        return false;
    }
}