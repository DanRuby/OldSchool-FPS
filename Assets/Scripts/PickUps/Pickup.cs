using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс для пикапов
/// Крутит и двигает пикапы
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class Pickup : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed=25;

    [SerializeField]
    private float verticalAmp=.25f;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Update()
    {

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, Mathf.Sin(Time.time) * verticalAmp * Time.deltaTime, 0));
    }

}
