using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed; // �̵� �ӵ�
    public float jumpPower;
    private Vector2 curMovementInput; // Input Action���� �޾ƿ� ��
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer; // ī�޶� �����̳�
    public float minXLook; // ī�޶� ȸ�� �ּҰ�
    public float maxXLook; // ī�޶� ȸ�� �ִ밪
    private float camCurXRot; // ī�޶��� ���� x�� ȸ�� ��
    public float lookSensitivity; // ȸ�� �ΰ���
    private Vector2 mouseDelta;


    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // ���콺 Ŀ�� ����� (Ŀ���� ���� ���� �߾ӿ� ����)
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

    void Move() // ������ �̵��� ������ �Լ�. 
    {
        // 1. ���� ���� : �յ�(y), �¿�(x)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; 
        // 2. �̵� 
        dir *= moveSpeed; // �̵� �ӵ� �ݿ�
        dir.y = _rigidbody.velocity.y; // ���� ���� ����

        _rigidbody.velocity = dir; // �̵� ����� �ݿ� ��.

    }

    void CameraLook() // ī�޶� ȸ��(���Ʒ� ������)
    {
        camCurXRot += mouseDelta.y * lookSensitivity;                                                       
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // ���� ���� 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // ī�޶��� ��/�Ʒ� ȸ�� ó��. 

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // ĳ������ ��/�� ȸ��                                                                                    
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)  // ��ư ������ ��
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
        if (context.phase == InputActionPhase.Started && IsGrounded()) // ���� ������� �� �۵��ϵ���.
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), //  ��������(Z�� ����)
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down), // ��������(Z���� �ݴ�)
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down), // ����������(x�� ����)
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down) // ��������(x���� �ݴ�)
        };

        for (int i = 0; i < rays.Length; i++) 
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) // groundLayer�� �ش��ϴ� �͸� ����
            {
                return true;
            }
        }

        return false;
    }
}