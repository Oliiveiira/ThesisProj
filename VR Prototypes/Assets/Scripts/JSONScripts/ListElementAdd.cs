using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class ListElementAdd : ProductListReader
{
    private Button button;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI pathContainers;

    [SerializeField]
    private ProductListWritter productList;

    [SerializeField]
    private IntSO recipeNumberSO;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void AddIngredientsToRecipe()
    {
        if (productList.myProductLists.recipes[recipeNumberSO.Value].ingredientsName.Count <= 4)
        {
            // Add the new ingredients to the current recipe
            productList.myProductLists.recipes[recipeNumberSO.Value].ingredientsName.Add(buttonText.text);
            productList.myProductLists.recipes[recipeNumberSO.Value].ingredientsPath.Add(pathContainers.text);

            //// Serialize and save the updated data to the JSON file
            productList.SaveProductListToJson();
            button.interactable = false;
        }
    }
}
