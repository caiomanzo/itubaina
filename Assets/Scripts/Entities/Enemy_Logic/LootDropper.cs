using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public GameObject lootPrefab;

    [Header("Configuração de Drop")]
    public int minDropValue = 1;
    public int maxDropValue = 5;

    public void Start()
    {
        // Adiciona um listener de "onDeath"
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

            GridEntity myEntity = GetComponent<GridEntity>();
            if (myEntity != null)
            {
                myEntity.ClearTiles();
            }

            GameObject spawnedLoot = Instantiate(lootPrefab, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity, transform.parent);

            ICollectible configurable = spawnedLoot.GetComponent<ICollectible>();
            if (configurable != null)
            {
                configurable.SetupValue(minDropValue, maxDropValue);
            }

            GridEntity lootEntity = spawnedLoot.GetComponent<GridEntity>();
            if (lootEntity != null && myEntity != null)
            {
                lootEntity.gridManager = myEntity.gridManager;
                lootEntity.rootPosition = gridPos;
                lootEntity.OccupyTiles();
            }
        }
    }
}
