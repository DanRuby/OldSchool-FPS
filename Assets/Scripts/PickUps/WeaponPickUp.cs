using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponPickUp : Pickup
{
    [SerializeField]
    private WeaponType weaponIndex;

    private void OnTriggerEnter(Collider other)
    {
        WeaponManager weaponManager= other.GetComponent<WeaponManager>();
        if(weaponManager!=null)
            weaponManager.PickUpWeapon(weaponIndex);
        Destroy(gameObject);
    }
}
