using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс передвигаемого в необходимом направлении объекта 
/// </summary>
public class MovingObject : MonoBehaviour
{
    private const float ERROR = .05F;
    
    [SerializeField]
    private Vector3 moveDirection;

    [SerializeField]
    private float moveSpeed;

    Vector3 desiredPosition;
    
    private void Awake()
    {
        desiredPosition = transform.position + moveDirection;
        enabled = false;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, desiredPosition) <= ERROR)
            enabled = false;
    }
}
