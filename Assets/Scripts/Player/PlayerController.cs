using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

/// <summary>
/// PlayerController Ŭ������ �÷��̾��� �̵�, ����, ��� �� ī�޶� ȸ���� �����մϴ�.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float dashForce;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask; // ���� ���̾� ����ũ
    public bool isDashing = false; // ��� ������ ����

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity; // ī�޶� ȸ�� �ΰ���
    private Vector2 mouseDelta;
    public bool canLook = true; // ī�޶� ȸ�� ���� ����

    public Action inventory;
    private Rigidbody rb;


    /// <summary>
    /// Awake �Լ��� ������ٵ� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Start �Լ��� Ŀ���� ��� ���·� �����մϴ�.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// FixedUpdate �Լ��� ���� ������ ó���ϸ�, �÷��̾ �̵���ŵ�ϴ�.
    /// </summary>
    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// LateUpdate �Լ��� ī�޶� ȸ���� ó���մϴ�.
    /// </summary>
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    /// <summary>
    /// �÷��̾ �̵���Ű�� �Լ��Դϴ�.
    /// </summary>
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;//������ ���� ���� ���Ʒ��� �������� �ż� y���� �״�� ������Ű�� ���ؼ� 

        rb.velocity = dir;
    }

    /// <summary>
    /// ���� �ð� ���� �÷��̾��� �̵� �ӵ��� ������Ű�� �ڷ�ƾ�Դϴ�.
    /// </summary>
    /// <param name="speed">������ �ӵ� ��</param>
    /// <param name="duration">���� �ð�</param>
    /// <returns>�ڷ�ƾ</returns>
    public IEnumerator SpeedBoostCoroutine(float speed, float duration)
    {
        isDashing = true;
        moveSpeed += speed;
        Debug.Log("�ӵ� ����! ���� �ӵ�: " + moveSpeed);
        yield return new WaitForSeconds(duration);

        moveSpeed -= speed;
        Debug.Log("�ӵ��������� ȿ���� ����Ǿ����ϴ�. ���� �ӵ�: " + moveSpeed);
        isDashing = false;
    }

    /// <summary>
    /// ī�޶� ȸ���� ó���ϴ� �Լ��Դϴ�.
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    /// <summary>
    /// �̵� �Է��� ó���ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
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
    /// ���콺 �̵� �Է��� ó���ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ���� �Է��� ó���ϴ� �Լ��Դϴ�.
    /// SpaceBar
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// ��� �Է��� ó���ϴ� �Լ��Դϴ�.
    /// Left Shift
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
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
    /// �÷��̾ ���鿡 �ִ��� Ȯ���ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <returns>���鿡 �ִ��� ����</returns>
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
    /// �κ��丮 �Է��� ó���ϴ� �Լ��Դϴ�.
    /// Tab
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    /// <summary>
    /// Ŀ�� ��� ���¸� ����ϴ� �Լ��Դϴ�.
    /// �κ��丮�� ���� ������ �� Ŀ�� ����� �����ϱ� ���� �Լ��Դϴ�. 
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
