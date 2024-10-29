using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt(); // 화면에 띄워줄 프롬프트 관련 함수
    public void OnInteract(); // 상호작용했을 때 어떤 효과를 발생시킬 건지

}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str; // 위 문자열 반환
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data; // 정보를 플레이어에 직접 전달하지 못해, CharacterManager를 통해 전달한다.
        CharacterManager.Instance.Player.addItem?.Invoke(); // addItem에 필요한 기능 구독
        Destroy(gameObject); // E키를 눌러 인벤토리로 이동하게 함 → 맵에는 사라지게 해야 함
    }
}
