using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProductListWritter : ProductListReader
{
    //private Recipes currentRecipe; // Reference to the current recipe
    private static Recipes currentRecipe; // Static variable to store the current recipe
    // public int recipeNumber = 0;
    [SerializeField]
    private IntSO recipeNumberSO;

    void Awake()
    {
        recipeNumberSO.Value = 0;
        myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);
        if(myProductLists.recipes.Count >= 2)
        {
           // RemoveRecipeAtIndex(myProductLists.recipes.Count - 2);
            myProductLists.recipes.RemoveRange(0, myProductLists.recipes.Count - 5);
        }

       // RemoveRecipeAtIndex(0);
        // If there are no recipes in the list, add an empty one      
        myProductLists.recipes.Add(new Recipes());

        // Set the current recipe to the first recipe in the list
        currentRecipe = myProductLists.recipes[0];
    }

    public void addAnotherRecipe()
    {
        // Debug log to check the count before adding a new recipe
        Debug.Log("Recipe count before adding: " + myProductLists.recipes.Count);
        myProductLists.recipes.Add(new Recipes());
        //myProductLists.recipes.Insert(0, new Recipes());
        // Debug log to check the count before adding a new recipe
        Debug.Log("Recipe count after adding: " + myProductLists.recipes.Count);
        // Set the current recipe to the first recipe in the list
        recipeNumberSO.Value++;
       // SwapElements(myProductLists.recipes, 0, recipeNumber + 1);
        //currentRecipe = myProductLists.recipes[0];
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

    public void SwapElements<T>(List<T> list, int index1, int index2)
    {
        if (index1 < 0 || index1 >= list.Count || index2 < 0 || index2 >= list.Count)
        {
            // Handle invalid indices based on your requirements
            return;
        }

        T temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    public void EraseList()
    {
        if (myProductLists.recipes.Count >= 2)
        {
            RemoveRecipeAtIndex(0);
            myProductLists.recipes.Add(new Recipes());

            // Set the current recipe to the first recipe in the list
           // currentRecipe = myProductLists.recipes[0];
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
