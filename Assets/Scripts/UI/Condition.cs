using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // Condition�� �ʿ��� ���� : 
    public float curValue; // ���� ��
    public float startValue; // ���� ��
    public float maxValue; // ������ �ִ밪
    public float passiveValue;
    public Image uiBar;


    void Start()
    {
        curValue = startValue; 
    }

    void Update()
    {
        // ui ������Ʈ
        uiBar.fillAmount = GetPercentage(); // curValue ���� ������Ʈ�� ���� Update���� ȣ��Ǵ� GetPercentage�� ���� ü�¹ٰ� ǥ���ȴ�.

    }

    float GetPercentage()
    {
        // ���� ���¿� ���� Fill Amount�� ǥ�� : curValue�� maxValue�� �������ش�.
        // Fill Amount�� ���� 0~1 = 0%~100%�� ����
        return curValue / maxValue;
    }

    // �� ������Ʈ�� ���� �Լ� Add()�� Subtratc(): ���� ���� ���ϰų� �� �� �Ű� ������ �־ ȣ���ϱ�   
    public void Add(float value)
    {
        // curValue += value; �ִ밪���� Ŀ�� �� ���� �� ����ó�� �ʿ�
        curValue = Mathf.Min(curValue + value, maxValue); // Mathf.Min( A , B) : A, B �� ���� ���� ������
                                                          // ���簪���� ������ ���� �ִ밪���� Ŀ�� �� �� maxValue�� ���õǾ� �ִ밪�� ���簪�� �־� ��
    }

    public void Subtract(float value)
    {
        // curValue -= value; �ּҰ����� �۾����� ��� �߻�
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
