using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : Pickup
{
    [SerializeField]
    private Keys key;

    public static event System.Action<Keys> KeyPickedup;

    private void OnTriggerEnter(Collider other)
    {
        KeysCollector keysCollector = other.GetComponent<KeysCollector>();
        if(keysCollector!=null)
        {
            keysCollector.AddKey(key);
            KeyPickedup?.Invoke(key);
            Destroy(gameObject);
        }
    }
}
