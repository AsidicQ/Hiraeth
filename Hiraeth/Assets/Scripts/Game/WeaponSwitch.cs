using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private WeaponEquipManager weaponEquipManager;

    private int currentWeaponIndex = 0;

    void Start()
    {
        EquipCurrentWeapon();
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleScrollWheel();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
    }

    private void HandleScrollWheel()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0f)
        {
            NextWeapon();
        }
        else if (scrollInput < 0f)
        {
            PreviousWeapon();
        }
    }

    private void NextWeapon()
    {
        currentWeaponIndex++;

        if (currentWeaponIndex >= weaponInventory.Weapons.Count)
        {
            currentWeaponIndex = 0;
        }

        EquipCurrentWeapon();
    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;

        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weaponInventory.Weapons.Count - 1;
        }

        EquipCurrentWeapon();
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex == currentWeaponIndex)
        {
            return;
        }

        if (weaponIndex < 0 || weaponIndex >= weaponInventory.Weapons.Count)
        {
            Debug.LogError("Weapon index out of range!");
            return;
        }

        currentWeaponIndex = weaponIndex;

        EquipCurrentWeapon();

    }

    private void EquipCurrentWeapon()
    {
        WeaponProfiles weaponToEquip = weaponInventory.GetWeapon(currentWeaponIndex);

        if (weaponToEquip == null)
        {
            return;
        }

        weaponEquipManager.EquipWeapon(weaponToEquip, weaponEquipManager.spawnOffset);
    }
}
