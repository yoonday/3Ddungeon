using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // Slot창에 넣어질 아이템의 정보
    public ItemData item;

    // 슬롯 UI에 필요한 변수들
    public Button button; 
    public Image icon; 
    public TextMeshProUGUI quantityText;  
    private Outline outline;

    //UIInventory에 대한 정보
    public UIInventory inventory;

    public int index; // 몇 번째 아이템 슬롯인지
    public bool equipped; // 장착 여부
    public int quantity; // 수량


    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        // 세팅해주는 함수
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon; // 스프라이트 넣어주기 (각 SO에 저장되어 있음)
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 숫자 1일때는 별도로 수량 표시 안해줌 
                                                                      

        if (outline != null) 
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        // 아이템을 버리거나 사용했을 때 사용
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        // 선택한 아이템
        inventory.SelectItem(index); // 나 자신의 인덱스를 넘겨주면 됨
    }
}
