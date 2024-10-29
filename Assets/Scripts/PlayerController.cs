using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed; // 이동 속도
    public float jumpPower;
    private Vector2 curMovementInput; // Input Action에서 받아올 값
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer; // 카메라 컨테이너
    public float minXLook; // 카메라 회전 최소값
    public float maxXLook; // 카메라 회전 최대값
    private float camCurXRot; // 카메라의 현재 x축 회전 값
    public float lookSensitivity; // 회전 민감도
    private Vector2 mouseDelta;


    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // 마우스 커서 숨기기 (커서를 게임 뷰의 중앙에 고정)
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move() // 실제로 이동을 수행할 함수. 
    {
        // 1. 방향 추출 : 앞뒤(y), 좌우(x)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; 
        // 2. 이동 
        dir *= moveSpeed; // 이동 속도 반영
        dir.y = _rigidbody.velocity.y; // 점프 상태 유지

        _rigidbody.velocity = dir; // 이동 결과를 반영 함.

    }

    void CameraLook() // 카메라 회전(위아래 움직임)
    {
        camCurXRot += mouseDelta.y * lookSensitivity;                                                       
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 범위 설정 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 카메라의 위/아래 회전 처리. 

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // 캐릭터의 좌/우 회전                                                                                    
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)  // 버튼 눌렀을 때
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
        if (context.phase == InputActionPhase.Started && IsGrounded()) // 땅에 닿아있을 때 작동하도록.
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), //  앞쪽으로(Z축 방향)
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), // 뒤쪽으로(Z축의 반대)
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down), // 오른쪽으로(x축 방향)
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down) // 왼쪽으로(x축의 반대)
        };

        for (int i = 0; i < rays.Length; i++) 
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) // groundLayer에 해당하는 것만 검출
            {
                return true;
            }
        }

        return false;
    }
}
