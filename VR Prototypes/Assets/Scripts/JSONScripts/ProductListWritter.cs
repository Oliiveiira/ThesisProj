using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProductListWritter : ProductListReader
{
    //private Recipes currentRecipe; // Reference to the current recipe
    private static Recipes currentRecipe; // Static variable to store the current recipe

    void Awake()
    {
        myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);
        RemoveRecipeAtIndex(0);

       // RemoveRecipeAtIndex(0);
        // If there are no recipes in the list, add an empty one      
        myProductLists.recipes.Add(new Recipes());

        // Set the current recipe to the first recipe in the list
        currentRecipe = myProductLists.recipes[0];
    }

    //public void AddIngredientsToRecipe()
    //{
    //    // Add the new ingredients to the current recipe
    //    currentRecipe.ingredientsName.Add(buttonText.text);
    //    currentRecipe.ingredientsPath.Add(pathContainers.text);

    //    // Serialize and save the updated data to the JSON file
    //    SaveProductListToJson();
    //}

    public void SaveProductListToJson()
    {
        string json = JsonUtility.ToJson(myProductLists);
        File.WriteAllText("Assets/Resources/Recipes/ProductsList.txt", json);
    }

    public void RemoveRecipeAtIndex(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < myProductLists.recipes.Count)
        {
            // Remove the element at the specified index
            myProductLists.recipes.RemoveAt(index);

            // Serialize and save the updated data to the JSON file
            SaveProductListToJson();
        }
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
