using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // Slotâ�� �־��� �������� ����
    public ItemData item;

    // ���� UI�� �ʿ��� ������
    public Button button; 
    public Image icon; 
    public TextMeshProUGUI quantityText;  
    private Outline outline;

    //UIInventory�� ���� ����
    public UIInventory inventory;

    public int index; // �� ��° ������ ��������
    public bool equipped; // ���� ����
    public int quantity; // ����


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
        // �������ִ� �Լ�
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon; // ��������Ʈ �־��ֱ� (�� SO�� ����Ǿ� ����)
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // ���� 1�϶��� ������ ���� ǥ�� ������ 
                                                                      

        if (outline != null) 
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        // �������� �����ų� ������� �� ���
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        // ������ ������
        inventory.SelectItem(index); // �� �ڽ��� �ε����� �Ѱ��ָ� ��
    }
}
