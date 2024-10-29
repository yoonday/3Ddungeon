using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PlayerController나 기능의 정보
    public PlayerController controller;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;                                                  
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
