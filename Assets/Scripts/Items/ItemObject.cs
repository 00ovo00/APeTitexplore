using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        if (data.type == ItemType.Readable)
            return str;
        // 아이템 타입이 Readable이 아닌 경우에만 안내 문구 출력
        string guideStr = "[E] 줍줍\n";
        return guideStr + str;
    }

    public void OnInteract()
    {
        // Readble 아이템이 인벤토리에 들어가지 않도록 제한
        if (data.type == ItemType.Readable) return;
        // Readble 아이템이 아니면 인벤토리에 아이템 넣고 Scene에서 없애기
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.OnAddItem?.Invoke();
        Destroy(gameObject);
    }
}