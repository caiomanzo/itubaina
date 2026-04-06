using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridEntity))]
public class Player : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    private GridEntity myEntity;

    [Header("Attributes")]
    public int maxHealth = 10;
    public int currentHealth;
    private Vector2Int gridPosition;

    [Header("Inventory Limits")]
    public int weaponLimit = 1;
    public int buddyLimit = 1;

    [Header("Equipped Items (Inventory)")]
    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<Buddy> equippedBuddies = new List<Buddy>();

    void Start()
    {
        currentHealth = maxHealth;
        myEntity = GetComponent<GridEntity>();

        gridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        myEntity.rootPosition = gridPosition;

        myEntity.OccupyTiles();

        UpdateVisualPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TryMove(Vector2Int.up);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TryMove(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TryMove(Vector2Int.right);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(Vector2Int.left);
    }

    // Func de movimentação do jogador no grid
    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPosition = gridPosition + direction;

        // Manager avalia quem está no tile
        if (gridManager.CheckWalkability(targetPosition, out GridEntity occupant))
        {
            myEntity.ClearTiles();

            gridPosition = targetPosition;
            myEntity.rootPosition = targetPosition;

            myEntity.OccupyTiles();
            UpdateVisualPosition();
        }
        else
        {
            if (occupant != null)
            {
                Debug.Log($"Bloqueado - Entidade: {occupant.name}");
            }
            else
            {
                Debug.Log("Bloqueado - Parede ou Limite");
            }
        }
    }

    void UpdateVisualPosition()
    {
        transform.position = new Vector3(gridPosition.x, gridPosition.y, 0f);
    }
}