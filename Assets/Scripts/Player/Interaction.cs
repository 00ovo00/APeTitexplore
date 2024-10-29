using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class Interaction : MonoBehaviour
{
    public float checkRate; // 상호작용 확인 빈도
    private float lastCheckTime;
    public float maxCheckDistance;
    private int interactsLayerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public PlayerController playerController;
    public TextMeshProUGUI promptText;
    private Camera camera;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        interactsLayerMask = 1 << LayerMask.NameToLayer("Interactable");
    }

    private void Start()
    {
        playerController.sheet += OnPopUpInput;
        camera = Camera.main;
        checkRate = 0.05f;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 스크린 기준 정중앙에서 ray 발사
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f));
            RaycastHit hit;

            // maxCheckDistance 내에 layerMask와 일치하여 raycast된 hit 정보 가져오기
            if(Physics.Raycast(ray, out hit, maxCheckDistance, interactsLayerMask))
            {
                // 현재 상호작용 중인 객체가 아니면(새로운 객체를 상호작용하는 경우)
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    // 현재 상호작용 중인 객체 정보 갱신하고 프롬프트 텍스트 설정
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            // raycast된 객체가 없으면
            else
            {
                // 현재 상호작용 중인 객체 null로 만들고 프롬프트의 텍스트 비활성화
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // E키가 눌렸고 상호작용 가능한 객체가 있을 때
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // 상호작용하는 함수 호출(상호작용 실행) 후
            curInteractable.OnInteract();
            // 상호작용 중인 객체가 없는 상태로 reset
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
    
    private void OnPopUpInput()
    {
        UIManager.Instance.ActivateSheetPanel();
        curInteractGameObject = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);
    }
}