//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.IO;

//public class ProductListToButton : ProductListReader
//{
//    public TextMeshProUGUI[] buttonText;
//    public TextMeshProUGUI[] pathContainers;

//    void Awake()
//    {
//        myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);
//       // AddIngredientsToRecipe();
//    }



//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            PopulateButtons();
//        }
//    }

//    private void PopulateButtons()
//    {
//        for(int i =0; i < buttonText.Length; i++)
//        {
//            buttonText[i].text = myProductLists.recipes[1].ingredientsName[i];
//            pathContainers[i].text = myProductLists.recipes[1].ingredientsPath[i];
//        }
//        RemoveRecipeAtIndex(1);
//    }

//    public void AddIngredientsToRecipe()
//    {
//        // Create a new Recipes object
//        Recipes newRecipe = new Recipes();

//        // Initialize the arrays if they are null
//        if (myProductLists.recipes == null)
//        {
//            myProductLists.recipes = new List<Recipes>();
//        }

//        for (int i = 0; i < buttonText.Length; i++)
//        {
//            myProductLists.recipes[0].ingredientsName[i] = buttonText[i].text;
//            myProductLists.recipes[0].ingredientsPath[i] = pathContainers[i].text;
//        }


//        //// Initialize the arrays with the same length as buttonText and pathContainers
//        //newRecipe.ingredientsName = new string[buttonText.Length];
//        //newRecipe.ingredientsPath = new string[pathContainers.Length];

//        //for (int i = 0; i < buttonText.Length; i++)
//        //{
//        //    newRecipe.ingredientsName[i] = buttonText[i].text;
//        //    newRecipe.ingredientsPath[i] = pathContainers[i].text;
//        //}

//        // Add the new Recipes object to the list
//        //  myProductLists.recipes.Add(newRecipe);

//        // Serialize and save the updated data to the JSON file
//        SaveProductListToJson();
//    }


//    // Add a function to save the ProductList to JSON
//    public void SaveProductListToJson()
//    {
//        string json = JsonUtility.ToJson(myProductLists);
//        File.WriteAllText("Assets/Resources/Recipes/ProductsList.txt", json);
//    }

//    public void RemoveRecipeAtIndex(int index)
//    {
//        // Check if the index is valid
//        if (index >= 0 && index < myProductLists.recipes.Count)
//        {
//            // Remove the element at the specified index
//            myProductLists.recipes.RemoveAt(index);

//            // Serialize and save the updated data to the JSON file
//            SaveProductListToJson();
//        }
//    }
//}
