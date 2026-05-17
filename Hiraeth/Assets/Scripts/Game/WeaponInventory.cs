using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private List<WeaponProfiles> startingWeapons;

    public IReadOnlyList<WeaponProfiles> Weapons => startingWeapons;

    public WeaponProfiles GetWeapon(int index)
    {
        if (index < 0 || index >= startingWeapons.Count)
        {
            Debug.LogError("Weapon index out of range!");
            return null;
        }
        return startingWeapons[index];
    }
}