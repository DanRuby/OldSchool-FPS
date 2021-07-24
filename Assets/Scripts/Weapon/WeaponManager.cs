using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Pistol,Shotgun,Rifle,RocketLauncher,Laser,Sniper}

/// <summary>
/// Менеджер оружий игрока
/// </summary>
public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponBase[] weapons;

    private bool[] pickedWeapons;

    private WeaponBase activeWeapon;
    private int activeWeaponIndex;

    public static event System.Action<WeaponBase> ActiveWeaponChanged;
    public static event System.Action<WeaponType> WeaponPickedup;

    private void Awake()
    {
        pickedWeapons = new bool[weapons.Length];
        for(int i=0; i<pickedWeapons.Length;i++)
        {
            if (pickedWeapons[i] == false)
                SetWeaponState(weapons[i], false);
        }
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            PickNextAvailableWeapon((a, b) => (a + b) % weapons.Length);
            return;
        }
        else if (scroll < 0)
        {
            PickNextAvailableWeapon((a, b) =>
            {
                int numb = (a - b) % weapons.Length;
                if (numb < 0)
                    numb = weapons.Length + numb;
                return numb;
            });
            return;
        }



        for (KeyCode buttonCode = KeyCode.Alpha1; buttonCode < KeyCode.Alpha1 + weapons.Length; buttonCode++)
        {
            int currentIndex = buttonCode - KeyCode.Alpha1;
            if (Input.GetKeyDown(buttonCode) && pickedWeapons[currentIndex] == true && activeWeaponIndex != currentIndex)
                SetNewActiveWeapon(weapons[currentIndex], currentIndex);
        }
    }

    /// <summary>
    /// Выбрать следующее активированное оружие
    /// </summary>
    /// <param name="search"></param>
    private void PickNextAvailableWeapon(System.Func<int,int,int> search)
    {
        if (!activeWeapon)
            return;

        for (int i = 1; i < weapons.Length; i++)
        {
            int calculatedIndex = search(activeWeaponIndex, i);
            if (pickedWeapons[calculatedIndex] && activeWeaponIndex != calculatedIndex)
            {
                SetNewActiveWeapon(weapons[calculatedIndex], calculatedIndex);
                return;
            }
        }
    }

    /// <summary>
    /// Установить состояние оружию
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="state"></param>
    private void SetWeaponState(WeaponBase weapon, bool state)
    {
        weapon.enabled = state;
        weapon.gameObject.SetActive(state);
    }

    /// <summary>
    /// Установить новое активное оружие
    /// </summary>
    /// <param name="newWeapon">Новое оружие</param>
    /// <param name="index">Индекс оружия в массиве</param>
    private void SetNewActiveWeapon(WeaponBase newWeapon,int index)
    {
        activeWeaponIndex = index;
        if (activeWeapon != null)
        {
            SetWeaponState(activeWeapon, false);
        }

        activeWeapon = newWeapon;
        SetWeaponState(activeWeapon, true);
        ActiveWeaponChanged?.Invoke(activeWeapon);
    }

    /// <summary>
    /// Реакция на пикап оружия
    /// </summary>
    /// <param name="id"></param>
    public void PickUpWeapon(WeaponType id)
    {
        WeaponPickedup?.Invoke(id);
        pickedWeapons[(int)id] = true;
        SetNewActiveWeapon(weapons[(int)id], (int)id);
    }

    public int AddAmmo(int amount, AmmoType ammoType)
    {
        return weapons[(int)ammoType].AddAmmo(amount);
    }
}
