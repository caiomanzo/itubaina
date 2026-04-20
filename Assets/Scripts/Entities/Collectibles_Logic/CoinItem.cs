using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public int minValue = 5;
    public int maxValue = 10;

    private int actualValue;

    void Start()
    {
        this.actualValue = Random.Range(minValue, maxValue + 1);
    }

    public bool Collect(Player player)
    {
        Debug.Log($"O jogador achou uma moeda valendo {actualValue}!");

        Inventory inv = player.GetComponent<Inventory>();
        inv.AddGold(actualValue);

        GetComponent<GridEntity>()?.ClearTiles();
        Destroy(gameObject);

        return true;
    }
}
