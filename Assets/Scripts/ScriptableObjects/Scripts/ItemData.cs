using UnityEngine;

public enum ItemType
{
    Resource,
    Consumable,
    Openable,
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
    //public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    // [Header("Equip")]
    // public GameObject equipPrefab;
    
    [Header("Open")]
    // public GameObject[] packedPrefab;
    public ItemData[] packedPrefab;
}