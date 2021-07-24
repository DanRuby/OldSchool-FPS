using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ������������ ������
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputs))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Acceleration")]
    [SerializeField]
    private float friction;

    [SerializeField]
    private float groundAccel;

    [SerializeField]
    private float maxGroundVel;

    [SerializeField]
    private float airAccel;

    [SerializeField]
    private float maxAirVel;

    [SerializeField]
    private LayerMask obstructionMask;

    [Space(7)]
    [Header("Jumping")]
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float initialFallVel;

    private const float GROUNDED_RAY = 0.05f;
    private const float AIR_RAY = 0.08f;

    [Space(7)]
    [Header("Mouse")]
    [SerializeField]
    float turnRate;

    private bool isGrounded;
    private Vector3 verticalVelocity;
    private Vector3 currentHorizontalVelocity;
    public Vector3 HorizontalVelocityVector => currentHorizontalVelocity;

    private PlayerInputs playerInputs;
    private CharacterController character;

    private void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
        character = GetComponent<CharacterController>();

        PlayerHealth.PlayerHit += OnPlayerHit;
        PlayerHealth.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
        PlayerHealth.PlayerHit -= OnPlayerHit;
        PlayerHealth.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerHit()
    {
        currentHorizontalVelocity *= .5f;
    }

    private void OnPlayerDied()
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        HandleGravityAndJump();
        Rotate();
        Move();
        HandleMovementObstructions();
    }

    /// <summary>
    /// �������� ��������� ������ � ������� ��� �� �����
    /// </summary>
    private void CheckGrounded()
    {
        //������� ��������� ��� �������� � ����������� �� ����������� ��������� ���������
        float chosenGroundCheckDistance = isGrounded ? character.skinWidth + GROUNDED_RAY : character.skinWidth + AIR_RAY;

        isGrounded = false;
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemishepre(),
                    character.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundLayer))
        {
            //�������, ��� ��� ����� ��� ���� ������������� �������� ������ ������ ���� �� ������� ���������� � ��� �� ����������� ��� � ������ ����� ������
            //� ���� ����������� ������ ���� ����� �������������� � �����������
            if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                IsNormalUnderSlopeLimit(hit.normal))
            {
                isGrounded = true;

                // handle snapping to the ground
                if (hit.distance > character.skinWidth)
                {
                    character.Move(Vector3.down * hit.distance);
                }
            }
        }
    }

    /// <summary>
    /// �������� ��������� ������ ������ ����� �������
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position - transform.up * (character.height / 2 - character.radius);
    }

    /// <summary>
    /// �������� ��������� ������ ������� ����� �������
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCapsuleTopHemishepre()
    {
        return transform.position + transform.up * (character.height / 2 - character.radius);
    }

    /// <summary>
    /// �������� �� ����������� ��� ������ ������ ��� ��������
    /// </summary>
    /// <param name="normal">������� �����������</param>
    /// <returns></returns>
    private bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= character.slopeLimit;
    }

    /// <summary>
    /// ������� ������ ��� ����������� ����
    /// </summary>
    private void Rotate()
    {
        float mouseInput = playerInputs.GetMouseX();
        float rotationY = mouseInput * (turnRate * Time.deltaTime);
        transform.Rotate(0, rotationY, 0, Space.Self);
    }

    /// <summary>
    /// �������� ������
    /// </summary>
    /// <returns></returns>
    private void HandleGravityAndJump()
    {
        bool jumpInput = playerInputs.GetJumpInput();
        if (isGrounded && jumpInput)
        {
            verticalVelocity = transform.up * Mathf.Sqrt(jumpForce * 2.0f * gravity);
        } else if (isGrounded)
            verticalVelocity = -transform.up * initialFallVel;
        else
            verticalVelocity -= transform.up * gravity * Time.deltaTime; ;
    }

    /// <summary>
    /// ������� �����������
    /// </summary>
    private void Move()
    {
        Vector3 keyboardInput = playerInputs.GetKeyboardInput();
        Vector3 accelDir = (keyboardInput.x * transform.right + keyboardInput.y * transform.forward).normalized;

        if (isGrounded)
            currentHorizontalVelocity = MoveGround(accelDir, currentHorizontalVelocity);
        else currentHorizontalVelocity = MoveAir(accelDir, currentHorizontalVelocity);

        character.Move((currentHorizontalVelocity + verticalVelocity) * Time.deltaTime);
    }

    /// <summary>
    /// ������ ����� ����������� �� ���� ������
    /// </summary>
    private void HandleMovementObstructions()
    {
        //���� ������� � ������� �������� ������
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemishepre(), character.radius, (currentHorizontalVelocity + verticalVelocity).normalized,
            out RaycastHit hit, (currentHorizontalVelocity + verticalVelocity).magnitude * Time.deltaTime, obstructionMask))
        {
            //���������� �������� �� ����������� � ������� ���������
            currentHorizontalVelocity = Vector3.ProjectOnPlane(currentHorizontalVelocity, hit.normal);
            currentHorizontalVelocity.y = 0;

            verticalVelocity = Vector3.ProjectOnPlane(verticalVelocity, hit.normal);
            verticalVelocity.x = 0;
            verticalVelocity.z = 0;
        }
    }

    #region quakelike acceleration

    /// <summary>
    /// ���������� ���������
    /// </summary>
    /// <param name="accelDir">��������������� �����������, � ������� ����� ����� ���������</param>
    /// <param name="prevVelocity">��������� �� ���������� �������� (��������� �������� ������)</param>
    /// <param name="accelerate">��������� ������ �� �����</param>
    /// <param name="maxVelocity">������������ �������� ������</param>
    /// <returns></returns>
    private Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float maxVelocity)
    {
        float projVel = Vector3.Dot(prevVelocity, accelDir); // Vector projection of Current velocity onto accelDir.
        float accelVel = accelerate * Time.fixedDeltaTime; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if (projVel + accelVel > maxVelocity)
            accelVel = maxVelocity - projVel;

        return prevVelocity + accelDir * accelVel;
    }

    /// <summary>
    /// ����������� �� �����
    /// </summary>
    /// <param name="accelDir"></param>
    /// <param name="prevVelocity"></param>
    /// <returns></returns>
    private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
    {
        // ���� ������������� �����
        float speed = prevVelocity.magnitude;
        if (speed != 0) // ���������� ������� �� 0
        {
            float drop = speed * friction * Time.fixedDeltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // ������������ ��������� �� ������ �������������
        }
        return Accelerate(accelDir, prevVelocity, groundAccel, maxGroundVel);
    }

    /// <summary>
    /// ����������� �� �������
    /// </summary>
    /// <param name="accelDir"></param>
    /// <param name="prevVelocity"></param>
    /// <returns></returns>
    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity) => Accelerate(accelDir, prevVelocity, airAccel, maxAirVel);

    #endregion

    public void AddExpForce(float expForce, float expRadius, Vector3 expPosition)
    {
        Vector3 accelDirection = transform.position - expPosition;
        float distanceFromCenter = accelDirection.magnitude;
        accelDirection = accelDirection.normalized;
        float expAccel = Map(distanceFromCenter, 0, expRadius, expForce, 0);

        Vector3 calculatedAccel = expAccel * accelDirection;
        verticalVelocity.y += calculatedAccel.y;
        currentHorizontalVelocity += calculatedAccel;
        currentHorizontalVelocity.y = 0;
    }

    private static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        x = Mathf.Clamp(x, in_min, in_max);
       return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    } 
}
