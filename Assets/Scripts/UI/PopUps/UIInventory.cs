using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    //public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
//    public ItemData defualtItem;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject openButton;
    // public GameObject equipButton;
    // public GameObject unEquipButton;
    // public GameObject dropButton;

    private Dictionary<ItemData, int> defualtItems;
    
    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Awake()
    {
        SetStartItemData();
    }

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
      //  dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
        AddStartingItems();
        UpdateUI();
    }

    private void SetStartItemData()
    {
        defualtItems = new Dictionary<ItemData, int>();
        ItemData itemData = Resources.Load("ItemDatas/HealthDrink") as ItemData;
        defualtItems.Add(itemData, 50);
        
        itemData = Resources.Load("ItemDatas/SizeDrink") as ItemData;
        defualtItems.Add(itemData, 10);
    }
    
    private void AddStartingItems()
    {
        int i = 0;
        foreach (var defualtItem in defualtItems)
        {
            slots[i].item = defualtItem.Key;
            slots[i].quantity = defualtItem.Value;
            i++;
        }
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        Debug.Log(data);
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            Debug.Log(slot.name);

            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        CharacterManager.Instance.Player.itemData = null;
    }
    
    public void AddItem(ItemData data)
    {
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        CharacterManager.Instance.Player.itemData = null;
    }

    public void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity <= data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for(int i = 0; i< selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        openButton.SetActive(selectedItem.item.type == ItemType.Openable);
        if (selectedItem.item.hasDuration && CharacterManager.Instance.Player.isBig)
            useButton.SetActive(false);
        else
            useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        openButton.SetActive(false);
        // equipButton.SetActive(false);
        // unEquipButton.SetActive(false);
        // dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if(selectedItem.item.type == ItemType.Consumable)
        {
            for(int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Duration:
                        CharacterManager.Instance.Player.isBig = true;
                        useButton.SetActive(false);
                        StartCoroutine(condition.ScaleChange(selectedItem.item.consumables[i].value, useButton));
                        break;
                }
            }
            RemoveSelctedItem();
        }
    }
    
    public void OnOpenButton()
    {
        if(selectedItem.item.type == ItemType.Openable)
        {
            for(int i = 0; i < selectedItem.item.packedPrefab.Length; i++)
            {
                // switch (selectedItem.item.packedPrefab[i].type)
                // {
                //     case ItemType.Consumable:
                //         break;
                //     case ItemType.Resource:
                //         StartCoroutine(condition.ScaleChange(selectedItem.item.consumables[i].value));
                //         break;
                // }
                AddItem(selectedItem.item.packedPrefab[i]);
            }
            RemoveSelctedItem();
        }
    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if(selectedItem.quantity <= 0)
        {
            selectedItem.item = null;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}