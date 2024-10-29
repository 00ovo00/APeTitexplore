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

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject openButton;

    private Dictionary<ItemData, int> defaultItems;
    
    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Awake()
    {
        SetStartItemData();
    }

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.OnAddItem += AddItem;

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
        // 시작 시 기본으로 인벤토리에 가지고 있는 아이템 정보 세팅
        defaultItems = new Dictionary<ItemData, int>();
        ItemData itemData = Resources.Load("ItemDatas/HealthDrink") as ItemData;
        defaultItems.Add(itemData, itemData.defaultAmount);
        
        itemData = Resources.Load("ItemDatas/SizeDrink") as ItemData;
        defaultItems.Add(itemData, itemData.defaultAmount);
    }
    
    private void AddStartingItems()
    {
        int i = 0;
        foreach (var defualtItem in defaultItems)
        {
            slots[i].item = defualtItem.Key;
            slots[i].quantity = defualtItem.Value;
            i++;
        }
    }

    private void Toggle()
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

    private void AddItem()
    {
        // 현재 상호작용 중인 아이템 정보 가져오기
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)  // 중복 가능한 아이템이면
        {
            ItemSlot slot = GetItemStack(data); // 아이템 정보 가져오기

            if(slot != null)    // 최대 개수보다 적으면
            {
                // 수량만 더해주고 UI 갱신
                slot.quantity++;
                UpdateUI();
                // 현재 상호작용 중인 아이템 없는 상태로 만듦
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        // 아이템 슬롯이 비어있으면 빈 슬롯 세팅
        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)   // 슬롯에 아이템이 있으면
        {
            // 아이템 데이터와 수량을 UI에 갱신
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        CharacterManager.Instance.Player.itemData = null;
    }
    
    private void AddItem(ItemData data)
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

    private void UpdateUI()
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
            // 아이템 데이터와 슬롯의 아이템이 같고 슬롯 수량이 최대값보다 작으면
            if (slots[i].item == data && slots[i].quantity <= data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    private ItemSlot GetEmptySlot()
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

        // 스탯 문자열은 공백으로 초기화
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // item이 consumable인 경우에 수치를 출력
        for(int i = 0; i< selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // 아이템이 Openable 타입이면 Openbutton 활성화
        openButton.SetActive(selectedItem.item.type == ItemType.Openable);
        // 아이템이 지속시간이 있고 플레이어가 커진 상태라면 UseButton 비활성화
        if (selectedItem.item.hasDuration && CharacterManager.Instance.Player.isBig)
            useButton.SetActive(false);
        else
            useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
    }

    private void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        openButton.SetActive(false);
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
                    case ConsumableType.Duration:   // 지속시간 타입의 아이템이면
                        CharacterManager.Instance.Player.isBig = true;  // 플레이어 커진 상태로 설정
                        useButton.SetActive(false); // 사용 버튼 비활성화(효과 발당 중 재사용 방지)
                        // 아이템 효과 적용하는 코루틴 시작
                        StartCoroutine(condition.ScaleChange(selectedItem.item.consumables[i].value, useButton));
                        break;
                }
            }
            RemoveSelctedItem();    // 사용하면 인벤토리에서 삭제
        }
    }
    
    public void OnOpenButton()
    {
        if(selectedItem.item.type == ItemType.Openable)
        {
            for(int i = 0; i < selectedItem.item.packedPrefab.Length; i++)
            {
                // 가지고 있는 아이템 정보를 인벤토리에 추가
                AddItem(selectedItem.item.packedPrefab[i]);
            }
            // 열고 나면 자기 자신은 인벤토리에서 없애기
            RemoveSelctedItem();
        }
    }

    private void RemoveSelctedItem()
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