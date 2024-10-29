using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 검출 빈도(업데이트 간격)
    private float lastCheckTime; // 마지막으로 확인한 시간
    public float maxCheckDistance; // 얼마나 멀리 있는 것을 체크할지
    public LayerMask layerMask; // 어떤 레이어가 달려있는 오브젝트를 추출할 것인지

  
    public GameObject curInteractGameObject; // 현재 상호작용한 게임 오브젝트의 정보
    private IInteractable curInteractable; // 인터페이스 캐싱


    public TextMeshProUGUI promptText; // 텍스트 출력
    private Camera camera; // 기준 카메라


    void Start()
    {
        camera = Camera.main; 
    }


    void Update()
    {
        // 매 프레임마다 업데이트 되지 않도록 빈도를 설정함
        if (Time.time - lastCheckTime > checkRate) 
        {
            lastCheckTime = Time.time;

            // 상호작용을 위한 Ray를 쏘기
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); 
            RaycastHit hit; 

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) 
            {
                //충돌됐을 때
                if (hit.collider.gameObject != curInteractGameObject) // 이미 상호작용하는 게임 오브젝트와 다를 때
                {
                    curInteractGameObject = hit.collider.gameObject; 
                    curInteractable = hit.collider.GetComponent<IInteractable>();

                    SetPromptText(); // 프롬프터 활성화
                }

            }
            else // 빈 공간에 Ray를 쏜 경우
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText() // 상호작용할 오브젝트가 있을 때 프롬프트 활성화
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context) // 상호작용 후 변수 초기화
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
