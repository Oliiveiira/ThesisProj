using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = System.Random;

public class Passcode : MonoBehaviour
{
    public string code;
    private string insertedCode = null;
    private int numberIndex;
    public TextMeshProUGUI uiCode;
    public GameObject codePanel;

    private void Start()
    {
        GenerateCode();
    }

    public void GenerateCode()
    {
        // Instantiate random number generator 
        Random random = new Random();

        // Print 4 random numbers between 50 and 100 
        for (int i = 1; i <= 4; i++)
         code = code + random.Next(0, 9).ToString();
        Debug.Log(code);
    }
        
    public void CodeFunction(string numbers)
    {
        numberIndex++;
        insertedCode = insertedCode + numbers;
        uiCode.text = uiCode.text + "*";
    }

    public void Enter() 
    {
        if(insertedCode == code)
        {
            codePanel.SetActive(false);
        }
    }

    public void Delete()
    {
        numberIndex++;
        insertedCode = null;
        uiCode.text = insertedCode;
    }
}
