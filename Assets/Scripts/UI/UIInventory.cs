using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    // UI 인벤토리가 켜질 때 기본 세팅

    // 아이템 슬롯들에 대한 정보
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel; // Slots 게임 오브젝트

    public Transform dropPosition;

    [Header("Select Item")] // 아이템 정보 텍스트
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    // 정보를 주고 받을 플레이어 컨트롤러, 컨디션
    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    void Start()
    {
        // 초기화 
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle; // 토글 함수 등록
        CharacterManager.Instance.Player.addItem += AddItem; // 아이템 저장 함수 등록

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount]; 

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    void Update()
    {

    }

    void ClearSelectedItemWindow() // 초기화 함수 
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle() // Tab키 눌러서 인벤토리창 활성화
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }

    }

    public bool IsOpen() // 창의 활성화 여부 확인
    {
        return inventoryWindow.activeInHierarchy; 
    }

    void AddItem() // 
    {
        ItemData data = CharacterManager.Instance.Player.itemData; // Player에 아이템 데이터를 넘겨받음

        // 1. 아이템 중복이 가능한지
        if (data.canStack)
        {
            // 쌓을 수 있다면 숫자 올려주기
            ItemSlot slot = GetItemStack(data); 
            if (slot != null) // 슬롯이 비어있지 않을 경우
            {
                slot.quantity++; // 수량 추가하기
                UpdateUI(); // UI 업데이트하기
                CharacterManager.Instance.Player.itemData = null; // 전달한 아이템 정보 초기화하기
                return;
            }
        }
        // 2. 중복이 불가능하다면 빈 슬롯 가져오기
        ItemSlot emptySlot = GetEmptySlot();
        // 2-1 빈 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI(); // UI 업데이트하기
            CharacterManager.Instance.Player.itemData = null; // 전달한 아이템 정보 초기화하기
            return;
        }
        // 2-2 빈 슬롯이 없다면 → 버리기
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI() // 슬롯 순회를 통해 업데이트
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null) 
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear(); 
            }
        }
    }


    ItemSlot GetItemStack(ItemData data)
    {
        // 조회하기
        for (int i = 0; i < slots.Length; i++)
        {
            // 중복된 아이템이 있고, 현재 수량이 최대 수량 이하일경우
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i]; 
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot() 
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i].item == null) 
            {
                return slots[i]; 
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        //  파괴했던 (ItemObject - OnInteract) 것을 다시 생성해야 함
        Instantiate(data.dropPrefabs, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index) // 아이템 슬롯에서 버튼을 눌렀을 때 호출되는 함수. 지금 슬롯이 몇 번째 인덱스인지 전달 →  slots 배열에 같은 인덱스에 해당하는 것 찾기
    {
        if (slots[index].item == null) return; // 선택한 아이템이 비어있을 때 return으로 탈출

        // 선택된 아이템에 대한 정보 캐싱해주기
        selectedItem = slots[index].item;
        selectedItemIndex = index;

        // 각 요소에 데이터 넣어주기
        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty; // 임시로 Empty로 해놓기 - 모든 아이템에 스탯이 있는 게 아님
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++) // 사용(소비) 가능한 아이템이 없다면(Length) for문 실행이 되지 않음
        {
            // selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n"; // type 추출하기. 여러개면 연속해서 등록해야해서 +=으로 이어서 넣어줌. \n는 엔터
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable); // 각 상태에 따라 활성화
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped); // 장착이 되어있지 않을 때 활성화
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped); // 장착이 되어있을 때 활성화
        dropButton.SetActive(true); // 조건 상관없이 무조건 활성화 

    }
}
