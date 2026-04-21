using UnityEngine;

[RequireComponent(typeof(GridEntity))]
public class Player : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    private GridEntity myEntity;

    [Header("Player Stats")]
    private Health myHealth;
    private Inventory myInventory;

    private Vector2Int gridPosition => myEntity.rootPosition;
    public Vector2Int previousGridPosition { get; private set; }

    void Awake()
    {
        myEntity = GetComponent<GridEntity>();
        myHealth = GetComponent<Health>();
        myInventory = GetComponent<Inventory>();
        previousGridPosition = gridPosition;
    }

    void Update()
    {
        if (gridManager == null) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TryMove(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TryMove(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TryMove(Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(Vector2Int.left);
    }

    void Walk(Vector2Int direction)
    {
        previousGridPosition = gridPosition;

        Vector2Int targetPosition = gridPosition + direction;

        myEntity.ClearTiles();
        myEntity.rootPosition = targetPosition;
        myEntity.OccupyTiles();
        UpdateVisualPosition();
    }

    public void UpdateVisualPosition()
    {
        transform.position = new Vector3(gridPosition.x, gridPosition.y, 0f);
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPosition = gridPosition + direction;
        bool actionTaken = false;

        // Atualiza a referencia do grid local
        gridManager = GetComponentInParent<GridManager>();
        if (gridManager == null) return;

        if (gridManager.CheckWalkability(targetPosition, out GridEntity occupant))
        {
            Walk(direction);
            actionTaken = true;
        }
        else if (occupant != null)
        {
            // LOGICA DE COLETA
            ICollectible item = occupant.GetComponent<ICollectible>();
            if (item != null)
            {
                if (item.Collect(this))
                {
                    Walk(direction);
                    actionTaken = true;
                }
            }
            else
            {
                // LOGICA DE COMBATE
                Health targetHealth = occupant.GetComponent<Health>();
                if (targetHealth != null)
                {
                    HandleCombat(targetHealth);
                    actionTaken = true;
                }
            }
        }

        // Processa o turno apos movimento do player
        if (actionTaken)
        {
            // Chama o EntityManager da sala
            EntityManager currentRoomManager = GetComponentInParent<EntityManager>();
            if (currentRoomManager != null)
            {
                currentRoomManager.ProcessEnemyTurns();
            }
        }
    }

    private void HandleCombat(Health target)
    {
        int damageToDeal = 1;

        if (myInventory != null && myInventory.weapons.Count > 0)
        {
            WeaponData activeWeapon = myInventory.weapons[0];
            damageToDeal = Mathf.Min(activeWeapon.durability, target.GetCurrentHealth());

            target.TakeDamage(damageToDeal);
            activeWeapon.durability -= damageToDeal;

            // Verifica se a arma quebrou
            if (activeWeapon.durability > 0)
            {
                Debug.Log($"{activeWeapon.weaponName} bateu! Durabilidade: {activeWeapon.durability}");
            }
            else
            {
                Debug.Log($"{activeWeapon.weaponName} quebrou!");
                myInventory.weapons.RemoveAt(0);
            }

            return;
        }

        // Player sem armas
        if (myHealth.GetCurrentHealth() > target.GetCurrentHealth())
        {
            damageToDeal = target.GetCurrentHealth();

            myHealth.TakeDamage(damageToDeal);
            target.TakeDamage(damageToDeal);
            return;
        }

        // Player tem vida menor que inimigo e morre
        myHealth.TakeDamage(myHealth.GetCurrentHealth());
    }
}