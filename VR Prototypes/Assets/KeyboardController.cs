using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public TMP_InputField textInput;
    private string insertedCode = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyLetter(string letter)
    {
        insertedCode = insertedCode + letter;
        textInput.text = insertedCode;
    }    
    
    public void Space()
    {
        insertedCode = insertedCode + " ";
        textInput.text = insertedCode;
    }

    public void EraseLastChar()
    {
        if (textInput.text.Length > 0)
        {
            textInput.text = textInput.text.Substring(0, textInput.text.Length - 1);
            insertedCode = insertedCode.Substring(0, insertedCode.Length - 1);
        }
    }
}
