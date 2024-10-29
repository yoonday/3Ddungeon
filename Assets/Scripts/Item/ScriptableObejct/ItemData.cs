using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable, // ���� ����
    Consumable, // �Һ� ����
    Resource // �ڿ�
}

public enum ConsumableType // �Һ��� �� ä������ ȿ��
{
    Health,
    Hunger
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType Type; // �Һ��� �� �ִ� ������ ����
    public float value; // �������� �Һ����� �� ȸ�������� ��
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // ������ �̸�
    public string description; // ������ ����
    public ItemType type; // ������ Ÿ��
    public Sprite icon; // ������ ����
    public GameObject dropPrefabs; // ������ ����

    [Header("Stacking")] 
    public bool canStack; 
    public int maxStackAmount; 

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
