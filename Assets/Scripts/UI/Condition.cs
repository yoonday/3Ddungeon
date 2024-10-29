using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // Condition에 필요한 변수 : 
    public float curValue; // 현재 값
    public float startValue; // 시작 값
    public float maxValue; // 상태의 최대값
    public float passiveValue;
    public Image uiBar;


    void Start()
    {
        curValue = startValue; 
    }

    void Update()
    {
        // ui 업데이트
        uiBar.fillAmount = GetPercentage(); // curValue 값의 업데이트에 따라 Update에서 호출되는 GetPercentage에 따라 체력바가 표현된다.

    }

    float GetPercentage()
    {
        // 현재 상태에 따라 Fill Amount로 표현 : curValue를 maxValue로 나누어준다.
        // Fill Amount의 범위 0~1 = 0%~100%와 같음
        return curValue / maxValue;
    }

    // 값 업데이트를 위한 함수 Add()와 Subtratc(): 현재 값에 더하거나 뺄 값 매개 변수로 넣어서 호출하기   
    public void Add(float value)
    {
        // curValue += value; 최대값보다 커질 수 있음 → 예외처리 필요
        curValue = Mathf.Min(curValue + value, maxValue); // Mathf.Min( A , B) : A, B 중 작은 값을 선택함
                                                          // 현재값에서 더해진 값이 최대값보다 커질 때 → maxValue가 선택되어 최대값이 현재값에 넣어 줌
    }

    public void Subtract(float value)
    {
        // curValue -= value; 최소값보다 작아지는 경우 발생
        curValue = Mathf.Max(curValue - value, 0);
    }

    public void SpeedUp(float amount, float duration, PlayerController controller)
    {
        StartCoroutine(ChangeSpeed(amount, duration, controller));
    }

    private IEnumerator ChangeSpeed(float amount, float duration, PlayerController controller)
    {
        controller.moveSpeed += amount;
        curValue = controller.moveSpeed;
        uiBar.fillAmount = GetPercentage();

        yield return new WaitForSeconds(duration);

        controller.moveSpeed -= amount;
        curValue = controller.moveSpeed;
        uiBar.fillAmount = GetPercentage();

    }

}
