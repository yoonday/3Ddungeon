using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    // UI �κ��丮�� ���� �� �⺻ ����

    // ������ ���Ե鿡 ���� ����
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel; // Slots ���� ������Ʈ

    [Header("Select Item")] // ������ ���� �ؽ�Ʈ
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    // ������ �ְ� ���� �÷��̾� ��Ʈ�ѷ�, �����
    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        // �ʱ�ȭ 
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        controller.inventory += Toggle; // ��� �Լ� ��������Ʈ�� ��� (�Լ��� ����ó�� ����ǰ� ��)
        // CharacterManager.Instance.Player.addItem += AddItem; // ������ ���� �Լ� ���

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

    void ClearSelectedItemWindow() // �ʱ�ȭ �Լ� 
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

    public void Toggle() // TabŰ ������ �κ��丮â Ȱ��ȭ�ϱ� �� PlayerController ��ũ��Ʈ�� OnInventory �Լ� ����� ����
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

    public bool IsOpen() // â�� Ȱ��ȭ ���θ� Ȯ���ϴ� �Լ�
    {
        return inventoryWindow.activeInHierarchy; // ���� ������ Ȱ��ȭ�Ǿ��ִ��� Ȯ���� �� �ִ� �Լ�
    }
}
