using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    // Variaveis de vida
    public int maxHealth = 10;
    private int currentHealth;

    // Evento de morte com listeners
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} tomou dano! HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<GridEntity>()?.ClearTiles();
        Destroy(gameObject);

        onDeath?.Invoke();
    }
}
