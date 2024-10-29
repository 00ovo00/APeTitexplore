using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce; // 스페이스 입력 시 점프 가중치
    public float jumpPadForce;  // 점프대에 적용되는 점프 가중치
    public LayerMask groundLayerMask;

    [Header("Size")]
    public Transform size;

    [Header("Look")]
    public Transform mainCamera;
    public float minXLook;  // 회전 범위 최소값
    public float maxXLook;  // 회전 범위 최대값
    private float camCurXRot;   // 현재 회전값
    public float lookSensitivity;   // 민감도
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true; // 카메라 고정, 커서 활성화 여부
    // true면 카메라 회전, 인벤토리 비활성화, 커서 비활성화
    // false면 카메라 고정, 인벤토리 활성화, 커서 활성화

    private Rigidbody rigidbody;

    public Action inventory;
    public Action sheet;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.OnGameOver += OnGameOverHandler;
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= OnGameOverHandler;
        }
    }

    private void FixedUpdate()
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

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        mainCamera.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private bool IsGrounded()
    {
        // 플레이어 기준 4방향 아래로 향하는 ray(책상 다리 형태)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            // 바닥에 Raycast되면 
            if (Physics.Raycast(rays[i], 0.6f, groundLayerMask))
            {
                return true;    // 땅에 붙어있는 상태로 인식
            }
        }
        return false;   // 공중에 떠있는 상태로 인식
    }
    
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        // Tab 키 눌렸으면
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    public void OnPopUpButton(InputAction.CallbackContext callbackContext)
    {
        // X 키 눌렸으면
        if (callbackContext.phase == InputActionPhase.Started)
        {
            sheet?.Invoke();
            ToggleCursor();
        }
    }

    public void ToggleCursor()
    {
        // 커서 상태 확인
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        // 락 상태이면 락 해제
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;  // 커서 활성화 여부 토글
    }
    
    private void OnGameOverHandler()
    {
        canLook = false;    // 카메라 고정
        Cursor.lockState = CursorLockMode.None; // 락 해제
        Cursor.visible = true;  // 커서 활성화
    }

    private void OnCollisionEnter(Collision other)
    {
        // 점프대 Collision 되면 jumpPadForce만큼 위로 순간적으로 힘을 가함
        if (other.gameObject.CompareTag("JumpPad"))
        {
            rigidbody.AddForce(Vector2.up * jumpPadForce, ForceMode.Impulse);
        }
    }
}