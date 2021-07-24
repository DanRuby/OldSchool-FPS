using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для управления фпс камеры (ротация вокруг оси x)
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float pitchSensetivity;

    [SerializeField]
    private float maxPitch;

    private float mouseInput;
    private float rotationX=0;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerHealth.PlayerDied += OnPlayerDead;
    }

    private void OnDestroy()
    {
        PlayerHealth.PlayerDied -= OnPlayerDead;
    }

    private void OnPlayerDead()
    {
        enabled = false;
    }

    void Update()
    {
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        mouseInput += Input.GetAxis("Mouse Y");
    }

    void FixedUpdate()
    {
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        rotationX -= mouseInput*( pitchSensetivity*Time.deltaTime);
        mouseInput = 0;
        rotationX = Mathf.Clamp(rotationX, -maxPitch, maxPitch);
        
        transform.localRotation= Quaternion.Euler(rotationX, 0, 0);
    }
}
