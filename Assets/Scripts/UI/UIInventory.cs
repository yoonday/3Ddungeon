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

    void Start()
    {
        // 초기화 
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        controller.inventory += Toggle; // 토글 함수 델리게이트로 등록 (함수가 변수처럼 저장되게 됨)
        // CharacterManager.Instance.Player.addItem += AddItem; // 아이템 저장 함수 등록

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

    public void Toggle() // Tab키 눌러서 인벤토리창 활성화하기 → PlayerController 스크립트에 OnInventory 함수 만들고 연결
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

    public bool IsOpen() // 창의 활성화 여부를 확인하는 함수
    {
        return inventoryWindow.activeInHierarchy; // 계층 구조에 활성화되어있는지 확인할 수 있는 함수
    }
}
