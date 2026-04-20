using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold = 0;

    public int maxWeapons = 2;
    public int maxBuddies = 1;

    public List<WeaponData> weapons = new List<WeaponData>();
    public List<string> buddies = new List<string>();

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"{gameObject.name} agora tem {gold} de ouro.");
    }

    public bool AddWeapon(WeaponData newWeapon)
    {
        if (weapons.Count >= maxWeapons)
        {
            Debug.Log("Inventário de armas cheio!");
            return false;
        }

        weapons.Add(newWeapon);
        Debug.Log($"Pegou {newWeapon.weaponName}. Arma ativa agora é: {weapons[0].weaponName}");
        return true;
    }

    public bool AddBuddy(string buddyName)
    {
        if (buddies.Count >= maxBuddies)
        {
            Debug.Log($"Você já tem o limite máximo de Buddies!");
            return false;
        }

        buddies.Add(buddyName);
        return true;
    }
}
