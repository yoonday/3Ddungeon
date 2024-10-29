using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable, // 장착 가능
    Consumable, // 소비 가능
}

public enum ConsumableType // 소비할 때 채워지는 효과
{
    Health,
    Hunger,
    Speed
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; // 소비할 수 있는 아이템 저장
    public float value; // 아이템을 소비했을 때 회복시켜줄 값
    public float duration; // 아이템 지속할 시간
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // 아이템 이름
    public string description; // 아이템 설명
    public ItemType type; // 아이템 타입
    public Sprite icon; // 아이콘 정보
    public GameObject dropPrefabs; // 프리팹 정보

    [Header("Stacking")] 
    public bool canStack; 
    public int maxStackAmount; 

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
