using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

    public void RegisterEnemy(EnemyBase enemy)
    {
        if (!activeEnemies.Contains(enemy)) activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyBase enemy)
    {
        if (activeEnemies.Contains(enemy)) activeEnemies.Remove(enemy);
    }

    public void ProcessEnemyTurns()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            EnemyBase enemy = activeEnemies[i];

            // Verifica se o inimigo não é nulo E se ainda está ativo na cena
            if (enemy != null && enemy.gameObject.activeInHierarchy && !enemy.IsDead())
            {
                enemy.TakeTurn();
            }
            else
            {
                // Se morreu ou sumiu e removido
                activeEnemies.RemoveAt(i);
            }
        }
    }
}