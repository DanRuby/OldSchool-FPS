using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Класс, выводящий информацию о текущем оружии
/// </summary>
public class WeaponsInfo : MonoBehaviour
{
    [SerializeField]
    private Image[] weaponImages;

    [SerializeField]
    private TextMeshProUGUI bulletsLeft;

    private AmmoType activeAmmo;
    private void Awake()
    {
        foreach (Image weapon in weaponImages)
            weapon.enabled = false;

        WeaponManager.WeaponPickedup += OnWeaponPickedUp;
        WeaponManager.ActiveWeaponChanged += OnActiveWeaponChanged;
        WeaponBase.WeaponShot += OnWeaponShot;
        WeaponBase.AmmoChange += OnAmmoChanged;
    }

    private void OnDestroy()
    {
        WeaponManager.WeaponPickedup -= OnWeaponPickedUp;
        WeaponManager.ActiveWeaponChanged -= OnActiveWeaponChanged;
        WeaponBase.WeaponShot -= OnWeaponShot;
        WeaponBase.AmmoChange -= OnAmmoChanged;
    }

    private void OnWeaponPickedUp(WeaponType id)
    {
        weaponImages[(int)id].enabled = true;
        activeAmmo = (AmmoType)(int)id;
    }

    private void OnActiveWeaponChanged(WeaponBase weapon)
    {
        bulletsLeft.text = GetBulletsText(weapon.Bullets);
        activeAmmo = weapon.AmmoType;
    }

    private void OnWeaponShot(WeaponEventParams eventParams) => bulletsLeft.text = GetBulletsText(eventParams.BulletsLeft);
    private void OnAmmoChanged(AmmoType type,int ammo)
    {
        if (type == activeAmmo)
            bulletsLeft.text=GetBulletsText(ammo);
    }

    private string GetBulletsText(int ammo) => ammo == WeaponBase.INFINITE_AMMO ? "Inf" : ammo.ToString();
}
