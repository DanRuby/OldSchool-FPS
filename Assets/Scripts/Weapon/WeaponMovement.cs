using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Добавление оружию небольших анимаций в зависимости от инпутов игрока
/// </summary>
public class WeaponMovement : MonoBehaviour
{
    [Tooltip("Трансформ оружия для анимации")]
    [SerializeField]
    private Transform gun;

    [Space(4)]
    [Header("Горизонтальное смещение")]
    [SerializeField]
    private float maxOffset;

    [SerializeField]
    private float speed;

    [Space(4)]
    [Header("Вращения")]
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float maxRotationY;

    [SerializeField]
    private float maxRotationX;

    private float gunRotationY = 0;
    private float gunRotationX = 0;
    private float kickbackSpeed = 0;
    private const float RATE_TO_KICKBACK = 2.5F;

    private void Awake()
    {
        WeaponBase.WeaponShot += OnWeaponShot;
    }

    private void OnDestroy()
    {
        WeaponBase.WeaponShot -= OnWeaponShot;
    }

    private void OnWeaponShot(WeaponEventParams eventParams)
    {
        gun.localPosition=new Vector3(gun.localPosition.x,gun.localPosition.y,-.3f);
        kickbackSpeed = eventParams.RateOfFire*RATE_TO_KICKBACK;
    }

    private void Update()
    {
        HandleKeyboardInput();
        HandleMouseInput();
    }

    /// <summary>
    /// Добавление ротации оружию в зависимости от движений мыши
    /// </summary>
    private void HandleMouseInput()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        gunRotationY += mouseInput.x;
        gunRotationY = ClampAndLerpAngle(gunRotationY, maxRotationY, rotationSpeed);

        gunRotationX -= mouseInput.y;
        gunRotationX = ClampAndLerpAngle(gunRotationX, maxRotationX, rotationSpeed);

        gun.transform.localEulerAngles = new Vector3(gunRotationX, gunRotationY, 0);
    }

    private float ClampAndLerpAngle(float inputRotation,float maxRotation,float lerpSpeed)
    {
        inputRotation = Mathf.Clamp(inputRotation, -maxRotation, maxRotation);
        return Mathf.Lerp(inputRotation, 0, Time.deltaTime * lerpSpeed);
    }

    /// <summary>
    /// Добавление смещения оружию в зависимости от горизонтального инипута клавиатуры
    /// </summary>
    private void HandleKeyboardInput()
    {
        
        Vector3 keyboardInput= new Vector3(Input.GetAxis("Horizontal"), 0);
        Vector3 lerped=Vector3.zero; 
        lerped.x= Mathf.Lerp(gun.localPosition.x, keyboardInput.x * maxOffset, Time.deltaTime * speed);
        lerped.z= Mathf.Lerp(gun.localPosition.z, 0, Time.deltaTime * kickbackSpeed);
        gun.localPosition = lerped;
    }
}