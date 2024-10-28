using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        if (data.type == ItemType.Readable)
            return str;
        string guideStr = "[E] 줍줍\n";
        return guideStr + str;
    }

    public void OnInteract()
    {
        if (data.type == ItemType.Readable) return;
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
    public void OnPopUp()
    {
        if (data.type != ItemType.Readable) return;
        CharacterManager.Instance.Player.itemData = data;
        
    }
}