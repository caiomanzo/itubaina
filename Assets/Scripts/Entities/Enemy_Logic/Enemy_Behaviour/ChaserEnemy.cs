using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    [Header("Chaser Stats")]
    public int attackDamage = 1;
    public int necessaryTimeToAttack = 1;

    private int timeSpentBesidesPlayer;

    public override void TakeTurn()
    {
        if (targetPlayer == null) return;

        // Checa se o player está adjacente
        if (GetGridDistanceToPlayer() == 1)
        {
            if (timeSpentBesidesPlayer >= necessaryTimeToAttack)
            {
                AttackPlayer();
                timeSpentBesidesPlayer = 0;
                return;
            }
            timeSpentBesidesPlayer++;
        }
        else
        {
            timeSpentBesidesPlayer = 0;

            // Tenta andar em direcao a posicao do player
            Vector2Int nextStep = FindNextStepTowardsPlayer();

            if (nextStep != myEntity.rootPosition)
            {
                TryMoveTo(nextStep);
            }
        }
    }

    public void AttackPlayer()
    {
        Health playerHealth = targetPlayer.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"Inimigo perseguiu e bateu! Causou {attackDamage} de dano.");
        }
    }

    // Algoritmo BFS para achar o shortest path ao jogador
    private Vector2Int FindNextStepTowardsPlayer()
    {
        Vector2Int startPos = myEntity.rootPosition;
        Vector2Int targetPos = targetPlayer.previousGridPosition;

        if (startPos == targetPos) return startPos;

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(startPos);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[startPos] = startPos;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == targetPos) break;

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                if (!cameFrom.ContainsKey(next))
                {
                    bool isWalkable = myEntity.gridManager.CheckWalkability(next, out GridEntity occupant);

                    // Permite andar se for chão livre ou se o player estiver no tile
                    if (isWalkable || next == targetPos || occupant == targetPlayer.GetComponent<GridEntity>())
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }
        }

        if (!cameFrom.ContainsKey(targetPos))
        {
            return startPos;
        }

        Vector2Int step = targetPos;
        while (cameFrom[step] != startPos)
        {
            step = cameFrom[step];
        }

        return step;
    }
}