using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmmoPickup : Pickup
{
    [SerializeField]
    private AmmoType ammoType;

    [SerializeField]
    private int amount;

    private void OnTriggerEnter(Collider other)
    {
        WeaponManager weaponManager = other.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            int leftAmmo = weaponManager.AddAmmo(amount, ammoType);
            if (leftAmmo > 0)
                amount = leftAmmo;
            else Destroy(gameObject);
        }
    }
}
