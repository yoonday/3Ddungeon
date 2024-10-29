using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition; // UICondition ��ü ����� ���� ����

    // ������ Condition�� �����Ͽ� ���� ������ �� �ִ�.
    Condition health { get { return uiCondition.health; } }  
    Condition hunger { get { return uiCondition.hunger; } }
    Condition speed { get { return uiCondition.Speed; } }


    public float noHungerHealthDecay; // ü���� ����ִ� ���� ���� 


    void Update()
    {
    
        hunger.Subtract(hunger.passiveValue * Time.deltaTime); 

        if (hunger.curValue == 0f) // hunger�� 0�� �� ü���� ���δ�.
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
        Debug.Log("���");
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
