using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс инпутов для передвижения от игрока
/// </summary>
public class PlayerInputs : MonoBehaviour
{
    private float mouseInput;
    private bool jumpInput;

    private void Update() => HandlePlayerInputs();

    private void HandlePlayerInputs()
    {
        mouseInput += Input.GetAxis("Mouse X");
        if (Input.GetButtonDown("Jump"))
            jumpInput = true;
    }

    public Vector3 GetKeyboardInput()=> new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    public bool GetJumpInput()
    {
        bool returnVal = jumpInput;
        jumpInput = false;
        return returnVal;
    }

    public float GetMouseX()
    {
        float returnVal = mouseInput;
        mouseInput = 0;
        return returnVal;
    }

}
