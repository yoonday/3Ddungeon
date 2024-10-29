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

    public Transform dropPosition;

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

    ItemData selectedItem;
    int selectedItemIndex = 0;

    void Start()
    {
        // �ʱ�ȭ 
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle; // ��� �Լ� ���
        CharacterManager.Instance.Player.addItem += AddItem; // ������ ���� �Լ� ���

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

    public void Toggle() // TabŰ ������ �κ��丮â Ȱ��ȭ
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

    public bool IsOpen() // â�� Ȱ��ȭ ���� Ȯ��
    {
        return inventoryWindow.activeInHierarchy; 
    }

    void AddItem() // 
    {
        ItemData data = CharacterManager.Instance.Player.itemData; // Player�� ������ �����͸� �Ѱܹ���

        // 1. ������ �ߺ��� ��������
        if (data.canStack)
        {
            // ���� �� �ִٸ� ���� �÷��ֱ�
            ItemSlot slot = GetItemStack(data); 
            if (slot != null) // ������ ������� ���� ���
            {
                slot.quantity++; // ���� �߰��ϱ�
                UpdateUI(); // UI ������Ʈ�ϱ�
                CharacterManager.Instance.Player.itemData = null; // ������ ������ ���� �ʱ�ȭ�ϱ�
                return;
            }
        }
        // 2. �ߺ��� �Ұ����ϴٸ� �� ���� ��������
        ItemSlot emptySlot = GetEmptySlot();
        // 2-1 �� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI(); // UI ������Ʈ�ϱ�
            CharacterManager.Instance.Player.itemData = null; // ������ ������ ���� �ʱ�ȭ�ϱ�
            return;
        }
        // 2-2 �� ������ ���ٸ� �� ������
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI() // ���� ��ȸ�� ���� ������Ʈ
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
        // ��ȸ�ϱ�
        for (int i = 0; i < slots.Length; i++)
        {
            // �ߺ��� �������� �ְ�, ���� ������ �ִ� ���� �����ϰ��
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
        //  �ı��ߴ� (ItemObject - OnInteract) ���� �ٽ� �����ؾ� ��
        Instantiate(data.dropPrefabs, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index) // ������ ���Կ��� ��ư�� ������ �� ȣ��Ǵ� �Լ�. ���� ������ �� ��° �ε������� ���� ��  slots �迭�� ���� �ε����� �ش��ϴ� �� ã��
    {
        if (slots[index].item == null) return; // ������ �������� ������� �� return���� Ż��

        // ���õ� �����ۿ� ���� ���� ĳ�����ֱ�
        selectedItem = slots[index].item;
        selectedItemIndex = index;

        // �� ��ҿ� ������ �־��ֱ�
        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty; // �ӽ÷� Empty�� �س��� - ��� �����ۿ� ������ �ִ� �� �ƴ�
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++) // ���(�Һ�) ������ �������� ���ٸ�(Length) for�� ������ ���� ����
        {
            // selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n"; // type �����ϱ�. �������� �����ؼ� ����ؾ��ؼ� +=���� �̾ �־���. \n�� ����
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable); // �� ���¿� ���� Ȱ��ȭ
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped); // ������ �Ǿ����� ���� �� Ȱ��ȭ
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped); // ������ �Ǿ����� �� Ȱ��ȭ
        dropButton.SetActive(true); // ���� ������� ������ Ȱ��ȭ 

    }
}
