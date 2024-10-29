using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISheet : MonoBehaviour
{
    public GameObject SheetPanel;
    public TMP_InputField keyInputField;
    private List<string> keyStringList; // 정답 문자열 저장하는 배열

    public Action OnSheetClose;

    private void Awake()
    {
        keyStringList = new List<string>();
    }

    private void Start()
    {
        keyStringList.Add("APT");   // 정답 문자
        keyStringList.Add("아파트");
        
        SheetPanel.SetActive(false);
    }

    public void OnConfirm()
    {
        // 아무것도 입력하지 않았으면 바로 리턴
        if (keyInputField == null) return;
        string inputStr = keyInputField.text.ToUpper();
        foreach (var str in keyStringList)
        {
            // 입력 문자열과 정답 문자열이 일치하면
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
