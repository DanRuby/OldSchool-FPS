using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField]
    private float amount;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if(playerHealth!=null)
        {
            if (playerHealth.Heal(amount))
                Destroy(this);
        }
    }

}
