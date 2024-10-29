using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition; // UICondition 객체 사용을 위한 선언

    // 각각의 Condition을 참조하여 값을 설정할 수 있다.
    Condition health { get { return uiCondition.health; } }  
    Condition hunger { get { return uiCondition.hunger; } }
    Condition speed { get { return uiCondition.Speed; } }


    public float noHungerHealthDecay; // 체력을 깎아주는 변수 설정 


    void Update()
    {
    
        hunger.Subtract(hunger.passiveValue * Time.deltaTime); 

        if (hunger.curValue == 0f) // hunger가 0일 때 체력이 깎인다.
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("사망");
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void SpeedUp(float amount)
    {
        speed.Add(amount);
    }
}
