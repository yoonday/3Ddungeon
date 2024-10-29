using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt(); // ȭ�鿡 ����� ������Ʈ ���� �Լ�
    public void OnInteract(); // ��ȣ�ۿ����� �� � ȿ���� �߻���ų ����

}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str; // �� ���ڿ� ��ȯ
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data; // ������ �÷��̾ ���� �������� ����, CharacterManager�� ���� �����Ѵ�.
        CharacterManager.Instance.Player.addItem?.Invoke(); // addItem�� �ʿ��� ��� ����
        Destroy(gameObject); // EŰ�� ���� �κ��丮�� �̵��ϰ� �� �� �ʿ��� ������� �ؾ� ��
    }
}
