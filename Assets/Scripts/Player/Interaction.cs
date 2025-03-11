using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interaction 클래스는 플레이어가 상호작용 가능한 객체와 상호작용할 수 있도록 합니다.
/// </summary>
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 상호작용 가능한 객체를 체크하는 주기
    private float lastCheckTime; // 마지막 체크 시간
    public float maxCheckDistance; // 상호작용 가능한 최대 거리
    public LayerMask layerMask; // 상호작용 가능한 레이어 마스크

    public GameObject curInteractGameObject; // 현재 상호작용 가능한 객체
    private IInteractable curInteractable; // 현재 상호작용 가능한 객체의 인터페이스

    public TextMeshProUGUI promptText;
    private Camera _camera;

    /// <summary>
    /// 시작 시 호출되는 함수로, 메인 카메라를 초기화합니다.
    /// </summary>
    void Start()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, 상호작용 가능한 객체를 체크합니다.
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
    /// 상호작용 프롬프트 텍스트를 설정합니다.
    /// </summary>
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    /// <summary>
    /// 상호작용 입력이 들어왔을 때 호출되는 함수로, 상호작용을 처리합니다.
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
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
