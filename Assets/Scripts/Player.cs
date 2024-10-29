using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // PlayerController나 기능의 정보
    public PlayerController controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;                                                  
        controller = GetComponent<PlayerController>(); 
    }
}
