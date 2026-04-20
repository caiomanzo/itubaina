using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public GameObject lootPrefab;

    public void Start()
    {
        Health healthComponent = GetComponent<Health>();

        if (healthComponent != null)
        {
            healthComponent.onDeath.AddListener(DropItem);
        }
    }

    public void DropItem()
    {
        if (lootPrefab != null)
        {
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y)
            );

            GameObject spawnedLoot = Instantiate(lootPrefab, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity);

            GridEntity lootEntity = spawnedLoot.GetComponent<GridEntity>();
            GridEntity myEntity = GetComponent<GridEntity>();

            if (lootEntity != null && myEntity != null)
            {
                lootEntity.gridManager = myEntity.gridManager;
                lootEntity.rootPosition = gridPos; // Usa a posicao calculada
                lootEntity.OccupyTiles();
            }
        }
    }
}
