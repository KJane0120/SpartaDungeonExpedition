using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interaction Ŭ������ �÷��̾ ��ȣ�ۿ� ������ ��ü�� ��ȣ�ۿ��� �� �ֵ��� �մϴ�.
/// </summary>
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // ��ȣ�ۿ� ������ ��ü�� üũ�ϴ� �ֱ�
    private float lastCheckTime; // ������ üũ �ð�
    public float maxCheckDistance; // ��ȣ�ۿ� ������ �ִ� �Ÿ�
    public LayerMask layerMask; // ��ȣ�ۿ� ������ ���̾� ����ũ

    public GameObject curInteractGameObject; // ���� ��ȣ�ۿ� ������ ��ü
    private IInteractable curInteractable; // ���� ��ȣ�ۿ� ������ ��ü�� �������̽�

    public TextMeshProUGUI promptText;
    private Camera _camera;

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, ���� ī�޶� �ʱ�ȭ�մϴ�.
    /// </summary>
    void Start()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ���, ��ȣ�ۿ� ������ ��ü�� üũ�մϴ�.
    /// </summary>
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = curInteractGameObject.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� ������Ʈ �ؽ�Ʈ�� �����մϴ�.
    /// </summary>
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    /// <summary>
    /// ��ȣ�ۿ� �Է��� ������ �� ȣ��Ǵ� �Լ���, ��ȣ�ۿ��� ó���մϴ�.
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);

        }
    }
}
