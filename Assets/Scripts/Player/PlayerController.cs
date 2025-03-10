using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float dashForce;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public bool isDashing = false;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()//���������� fixedupdate���� �ؾ���
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;//������ ���� ���� ���Ʒ��� �������� �ż� y���� �״�� ������Ű�� ���ؼ� 

        rb.velocity = dir;
    }


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


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

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
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

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

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
