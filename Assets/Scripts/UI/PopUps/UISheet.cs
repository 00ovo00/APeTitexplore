using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISheet : MonoBehaviour
{
    public GameObject SheetPanel;
    public TMP_InputField keyInputField;
    private List<string> keyStringList;

    public Action OnSheetClose;

    private void Awake()
    {
        keyStringList = new List<string>();
    }

    void Start()
    {
        keyStringList.Add("APT");   
        keyStringList.Add("아파트");
        
        SheetPanel.SetActive(false);
    }

    void Update()
    {
        
    }

    public void OnConfirm()
    {
        if (keyInputField == null) return;
        string inputStr = keyInputField.text.ToUpper();
        foreach (var str in keyStringList)
        {
            Debug.Log(str);
            Debug.Log(str == inputStr);
            if (str == inputStr)
            {
                GameManager.Instance.GameClear(); 
                return;
            }
        }
    }

    public void OnCancel()
    {
        UIManager.Instance.DeActivateSheetPanel();
        CharacterManager.Instance.Player.controller.ToggleCursor();
    }
}
