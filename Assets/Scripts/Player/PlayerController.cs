using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/// <summary>
/// PlayerController 클래스는 플레이어의 이동, 점프, 대시 및 카메라 회전을 관리합니다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float dashForce;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask; // 지면 레이어 마스크
    public bool isDashing = false; // 대시 중인지 여부

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity; // 카메라 회전 민감도
    private Vector2 mouseDelta;
    public bool canLook = true; // 카메라 회전 가능 여부

    public Action inventory;
    private Rigidbody rb;


    /// <summary>
    /// Awake 함수는 리지드바디를 초기화합니다.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Start 함수는 커서를 잠금 상태로 설정합니다.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// FixedUpdate 함수는 물리 연산을 처리하며, 플레이어를 이동시킵니다.
    /// </summary>
    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// LateUpdate 함수는 카메라 회전을 처리합니다.
    /// </summary>
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    /// <summary>
    /// 플레이어를 이동시키는 함수입니다.
    /// </summary>
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;//점프를 했을 때만 위아래로 움직여야 돼서 y값을 그대로 유지시키기 위해서 

        rb.velocity = dir;
    }

    /// <summary>
    /// 일정 시간 동안 플레이어의 이동 속도를 증가시키는 코루틴입니다.
    /// </summary>
    /// <param name="speed">증가할 속도 값</param>
    /// <param name="duration">지속 시간</param>
    /// <returns>코루틴</returns>
    public IEnumerator SpeedBoostCoroutine(float speed, float duration)
    {
        isDashing = true;
        moveSpeed += speed;
        Debug.Log("속도 증가! 현재 속도: " + moveSpeed);
        yield return new WaitForSeconds(duration);

        moveSpeed -= speed;
        Debug.Log("속도아이템의 효과가 종료되었습니다. 현재 속도: " + moveSpeed);
        isDashing = false;
    }

    /// <summary>
    /// 카메라 회전을 처리하는 함수입니다.
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    /// <summary>
    /// 이동 입력을 처리하는 함수입니다.
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// 마우스 이동 입력을 처리하는 함수입니다.
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 점프 입력을 처리하는 함수입니다.
    /// SpaceBar
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 대시 입력을 처리하는 함수입니다.
    /// Left Shift
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isDashing == false)
        {
            if (context.control.displayName == "Shift")
            {
                CharacterManager.Instance.Player.condition.UseStamina(15);
                StartCoroutine(SpeedBoostCoroutine(dashForce, 3));
            }
        }
    }

    /// <summary>
    /// 플레이어가 지면에 있는지 확인하는 함수입니다.
    /// </summary>
    /// <returns>지면에 있는지 여부</returns>
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward *0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward *0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right *0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right *0.2f) + (transform.up * 0.01f), Vector3.down),
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 인벤토리 입력을 처리하는 함수입니다.
    /// Tab
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    /// <summary>
    /// 커서 잠금 상태를 토글하는 함수입니다.
    /// 인벤토리가 열린 상태일 때 커서 잠금을 해제하기 위한 함수입니다. 
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
