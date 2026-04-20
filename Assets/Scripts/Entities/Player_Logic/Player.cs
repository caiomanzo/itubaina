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

    void Awake()
    {
        myEntity = GetComponent<GridEntity>();
        myHealth = GetComponent<Health>();
        myInventory = GetComponent<Inventory>();
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
        Vector2Int targetPosition = gridPosition + direction;

        myEntity.ClearTiles(); // Limpa posicao antiga no dicionário

        // Atualiza a posicao
        myEntity.rootPosition = targetPosition;

        myEntity.OccupyTiles(); // Registra nova posicao
        UpdateVisualPosition();
    }

    public void UpdateVisualPosition()
    {
        transform.position = new Vector3(gridPosition.x, gridPosition.y, 0f);
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPosition = gridPosition + direction;

        if (gridManager.CheckWalkability(targetPosition, out GridEntity occupant))
        {
            Walk(direction);
        }
        else if (occupant != null)
        {
            Debug.Log($"Bati em algo: {occupant.name}");
            // LÓGICA DE COLETA
            ICollectible item = occupant.GetComponent<ICollectible>();
            if (item != null)
            {
                if (item.Collect(this)) Walk(direction);
                return;
            }

            // LÓGICA DE COMBATE
            Health targetHealth = occupant.GetComponent<Health>();
            if (targetHealth != null)
            {
                HandleCombat(targetHealth);
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

        // Player sem armas, toma dano tambem, mas tem vida maior
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