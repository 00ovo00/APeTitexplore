using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public bool isBig;  // 플레이어가 커진 상태인지 확인
    
    public ItemData itemData;
    
    public Action OnAddItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        isBig = false;
    }
}