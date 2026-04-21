using UnityEngine;

public class WeaponItem : MonoBehaviour, ICollectible
{

    public WeaponData weaponStats;

    public void SetupValue(int min, int max)
    {
        if (weaponStats != null)
        {
            weaponStats.durability = Random.Range(min, max + 1);
        }
    }

    public bool Collect(Player player)
    {
        Inventory inv = player.GetComponent<Inventory>();

        if (inv != null)
        {
            if (inv.AddWeapon(weaponStats))
            {
                GetComponent<GridEntity>()?.ClearTiles();
                Destroy(gameObject);

                return true;
            }
        }

        return false;
    }
}
