using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс лазера, которым стреляет LaserWeapon
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField]
    private float startWidth;

    [SerializeField]
    private float disappearSpeed;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        line.startWidth = startWidth;
    }

    void Update()
    {
        line.startWidth-= (Time.deltaTime*disappearSpeed);
        if (line.startWidth <= 0)
            enabled = false;
    }
}
