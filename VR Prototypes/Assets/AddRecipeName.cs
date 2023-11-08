using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddRecipeName : ProductListReader
{
    public TextMeshProUGUI recipe;

    [SerializeField]
    private ProductListWritter productList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecipeNameAdd()
    {
        productList.myProductLists.recipes[0].recipeName = recipe.text;
    }
}
