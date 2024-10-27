using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISheet : MonoBehaviour
{
    public InputField keyInputField;
    private List<string> keyStringList;

    private void Awake()
    {
        keyInputField = GetComponent<InputField>();
        keyStringList = new List<string>();
    }

    void Start()
    {
        keyStringList.Add("APT");   
        keyStringList.Add("아파트");   
    }

    void Update()
    {
        
    }

    public void OnConfirm()
    {
        string inputStr = keyInputField.text.ToUpper();
        foreach (var str in keyStringList)
        {
            if (str == inputStr)
            {
                GameManager.Instance.GameClear(); 
                return;
            }
        }
    }
}
