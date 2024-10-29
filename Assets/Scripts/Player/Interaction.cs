using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // ���� ��(������Ʈ ����)
    private float lastCheckTime; // ���������� Ȯ���� �ð�
    public float maxCheckDistance; // �󸶳� �ָ� �ִ� ���� üũ����
    public LayerMask layerMask; // � ���̾ �޷��ִ� ������Ʈ�� ������ ������

  
    public GameObject curInteractGameObject; // ���� ��ȣ�ۿ��� ���� ������Ʈ�� ����
    private IInteractable curInteractable; // �������̽� ĳ��


    public TextMeshProUGUI promptText; // �ؽ�Ʈ ���
    private Camera camera; // ���� ī�޶�


    void Start()
    {
        camera = Camera.main; 
    }


    void Update()
    {
        // �� �����Ӹ��� ������Ʈ ���� �ʵ��� �󵵸� ������
        if (Time.time - lastCheckTime > checkRate) 
        {
            lastCheckTime = Time.time;

            // ��ȣ�ۿ��� ���� Ray�� ���
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); 
            RaycastHit hit; 

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) 
            {
                //�浹���� ��
                if (hit.collider.gameObject != curInteractGameObject) // �̹� ��ȣ�ۿ��ϴ� ���� ������Ʈ�� �ٸ� ��
                {
                    curInteractGameObject = hit.collider.gameObject; 
                    curInteractable = hit.collider.GetComponent<IInteractable>();

                    SetPromptText(); // �������� Ȱ��ȭ
                }

            }
            else // �� ������ Ray�� �� ���
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText() // ��ȣ�ۿ��� ������Ʈ�� ���� �� ������Ʈ Ȱ��ȭ
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context) // ��ȣ�ۿ� �� ���� �ʱ�ȭ
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null) 
        {
            curInteractable.OnInteract(); 
            curInteractGameObject = null; 
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
