using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateButtons : MonoBehaviour
{
    public Button[] uiButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateBtns()
    {
        for(int i =0; i < uiButtons.Length; i++)
        {
            uiButtons[i].interactable = true;
        }
    }
}
