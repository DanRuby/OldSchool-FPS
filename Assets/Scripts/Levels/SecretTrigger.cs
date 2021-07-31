using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Триггер секрета
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class SecretTrigger : MonoBehaviour
{
    public static event System.Action SecretFound;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            SecretFound?.Invoke();
            Destroy(gameObject);
        }
    }
}
