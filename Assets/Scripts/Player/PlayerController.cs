using System;
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
    
    public bool canLook = true;

    [Header("Camera")] // ī�޶� 
    public Camera FPCamera; // 1��Ī
    public Camera TPCamera; // 3��Ī
    public Interaction interaction;

    private bool isThirdPerson = false; // 1��Ī�� �⺻ ������

    public Action inventory; // �κ��丮 Ȱ��ȭ ����

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
        if (canLook)
        {
            CameraLook();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) // ALTŰ�� ī�޶� ��ȯ
        {
            SwitchCamera();
        }
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

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke(); // UIinventory�� ��ϵ� Toggle ȣ�� - TabŰ�� â Ȱ��ȭ
            ToggleCursor(); // ����� �Ǿ��ִٸ� 
        }
    }

    public void ToggleCursor()// Ŀ�� ������ִ� ���
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; 
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle; //  ��� Ȱ��ȭ �� ȭ�� ��� ����
    }

    private void SwitchCamera()
    {
        if (isThirdPerson)
        {
            SwitchToFirstPerson();
        }
        else
        {
            SwitchToThirdPerson();
        }
        isThirdPerson = !isThirdPerson;
    }

    private void SwitchToFirstPerson()
    {
        FPCamera.enabled = true;
        TPCamera.enabled = false;
        interaction.SetCamera(FPCamera); // Interaction�� ī�޶� ����
    }

    private void SwitchToThirdPerson()
    {
        FPCamera.enabled = false;
        TPCamera.enabled = true;
        interaction.SetCamera(TPCamera); // Interaction�� ī�޶� ����
    }

}
