using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;

    [SerializeField]
    private GameObject moneyPanel;    
    [SerializeField]
    private GameObject ingredientsPanel;
    [SerializeField]
    private GameObject warning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void CheckIfIngredientsSelected()
    {
        if (CheckIfButtonPressed())
        {
            ingredientsPanel.SetActive(false);
            moneyPanel.SetActive(true);
        }
        else
        {
            warning.SetActive(true);
        }
    }

    bool CheckIfButtonPressed()
    {
        foreach(var button in buttons)
        {
            if(button.enabled == false)
            {
                return false;
            }
        }
        return true;
    }

}
