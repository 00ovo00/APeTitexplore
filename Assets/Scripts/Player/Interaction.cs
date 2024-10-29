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

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, maxCheckDistance, interactsLayerMask))
            {
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                    return;
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

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

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
    
    private void OnPopUpInput()
    {
        UIManager.Instance.ActivateSheetPanel();
        curInteractGameObject = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);
    }
}