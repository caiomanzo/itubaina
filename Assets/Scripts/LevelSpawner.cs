using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Referências")]
    public GridManager gridManager;
    public GameObject mainMenuUI;

    [Header("Prefabs de Infraestrutura")]
    public GameObject mapPrefab;

    [Header("Prefabs de Entidades")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject swordPrefab;

    [Header("Configurações de Jogo")]
    public int enemyCount = 3;
    public int swordCount = 2;

    // Lista de coordenadas de spawn do scan
    private List<Vector2Int> validTiles = new List<Vector2Int>();

    public void StartGame()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);

        gridManager.ClearAllEntities();

        // Instancia o mapa
        Instantiate(mapPrefab, Vector3.zero, Quaternion.identity, gridManager.transform);

        // Escaneia o mapa para identificar chao e parede
        ScanAvailableSpace();

        // Spawna o player
        Vector2Int playerStartPos = new Vector2Int(0, 0);
        if (!validTiles.Contains(playerStartPos)) playerStartPos = GetRandomValidTile();

        SpawnAt(playerPrefab, playerStartPos);
        validTiles.Remove(playerStartPos);

        // Spawna Inimigos
        for (int i = 0; i < enemyCount; i++)
        {
            if (validTiles.Count > 0) SpawnInRandomFreeTile(enemyPrefab);
        }

        // Spawna Armas
        for (int i = 0; i < swordCount; i++)
        {
            if (validTiles.Count > 0) SpawnInRandomFreeTile(swordPrefab);
        }
    }

    private void ScanAvailableSpace()
    {
        validTiles.Clear();

        for (int x = -25; x <= 25; x++)
        {
            for (int y = -25; y <= 25; y++)
            {
                Vector2Int p = new Vector2Int(x, y);

                // Checagem de void
                if (gridManager.CheckWalkability(p, out _))
                {
                    if (Physics2D.OverlapPoint(new Vector2(x, y)) != null)
                    {
                        validTiles.Add(p);
                    }
                }
            }
        }
        Debug.Log($"Scan concluído. {validTiles.Count} espaços seguros encontrados.");
    }

    private void SpawnInRandomFreeTile(GameObject prefab)
    {
        Vector2Int randomPos = GetRandomValidTile();
        SpawnAt(prefab, randomPos);
        validTiles.Remove(randomPos); // Ocupa o espaço
    }

    private Vector2Int GetRandomValidTile()
    {
        int index = Random.Range(0, validTiles.Count);
        return validTiles[index];
    }

    private void SpawnAt(GameObject prefab, Vector2Int position)
    {
        GameObject newObj = Instantiate(prefab, new Vector3(position.x, position.y, 0), Quaternion.identity, gridManager.transform);

        GridEntity entity = newObj.GetComponent<GridEntity>();
        if (entity != null)
        {
            entity.gridManager = this.gridManager;
            entity.rootPosition = position;
            entity.OccupyTiles();
        }

        Player p = newObj.GetComponent<Player>();
        if (p != null)
        {
            p.gridManager = this.gridManager;
            p.UpdateVisualPosition();
        }
    }
}