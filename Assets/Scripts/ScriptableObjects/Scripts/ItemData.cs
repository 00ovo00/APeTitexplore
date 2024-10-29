using UnityEngine;

public enum ItemType
{
    Resource,
    Consumable,
    Openable,
    Readable,
}

public enum ConsumableType
{
    Health,
    Duration,
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public bool hasDuration;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
    public int defaultAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Open")]
    public ItemData[] packedPrefab;
}