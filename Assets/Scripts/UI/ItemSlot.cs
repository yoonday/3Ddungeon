using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    // Slot창에 넣어질 아이템의 정보
    public ItemData item;

    //UIInventory에 대한 정보
    public UIInventory inventory;

    public int index; // 몇 번째 아이템 슬롯인지
    public bool equipped; // 장착 여부
    public int quantity; // 수량


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
