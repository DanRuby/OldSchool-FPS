using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Класс объекта, с которым игрок может взаимодействовать
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    [SerializeField,Tooltip("Событие вызывается при успещном взаимодействии")]
    private UnityEvent OnActivated;

    [SerializeField]
    private Keys requiredKey;

    private Camera cam;
    private const float DEGREES = 45;
    private void Awake()
    {
        cam = Camera.main;
        enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)&&ShouldActivate())
        {
            float angle = Vector3.Angle(Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up), -transform.forward);
            if (angle< DEGREES)
                Activate();
        }
    }

    private bool ShouldActivate()
    {
        if (requiredKey == Keys.None)
            return true;
        return KeysCollector.CollectedKeys[(int)requiredKey] == true;
    }

    private void Activate()
    {
        OnActivated.Invoke();
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>()!=null)
            enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
            enabled = false;
    }
}
