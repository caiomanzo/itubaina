using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{

    private int dropQuantity;

    public void SetupValue(int min, int max)
    {
        dropQuantity = Random.Range(min, max + 1);
    }

    public bool Collect(Player player)
    {
        Debug.Log($"O jogador achou uma moeda valendo {dropQuantity}!");

        Inventory inv = player.GetComponent<Inventory>();
        inv.AddGold(dropQuantity);

        GetComponent<GridEntity>()?.ClearTiles();
        Destroy(gameObject);

        return true;
    }
}
