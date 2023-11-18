using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PuzzleListWritter : PuzzleListReader
{
    [SerializeField]
    private IntSO puzzleLevelSO;

    void Awake()
    {
        puzzleLevelSO.Value = 0;
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "ImageLinks.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myPuzzleList = JsonUtility.FromJson<PuzzleList>(jsonText);

        ////myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);
        //if (myProductLists.recipes.Count >= 2)
        //{
        //    // RemoveRecipeAtIndex(myProductLists.recipes.Count - 2);
        //    myProductLists.recipes.RemoveRange(0, myProductLists.recipes.Count - 5);
        //}
        //RemovePuzzleAtIndex(0);
        myPuzzleList.puzzlelevel.Clear();

        // RemoveRecipeAtIndex(0);
        // If there are no recipes in the list, add an empty one      
        myPuzzleList.puzzlelevel.Add(new PuzzleLevel());
    }

    public void addAnotherPuzzleLevel()
    {
        // Debug log to check the count before adding a new recipe
        Debug.Log("Recipe count before adding: " + myPuzzleList.puzzlelevel.Count);
        myPuzzleList.puzzlelevel.Add(new PuzzleLevel());

        // Debug log to check the count before adding a new recipe
        Debug.Log("Recipe count after adding: " + myPuzzleList.puzzlelevel.Count);
        // Set the current recipe to the first recipe in the list
        puzzleLevelSO.Value++;
    }

    public void SavePuzzleListToJson()
    {
        string json = JsonUtility.ToJson(myPuzzleList);
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "ImageLinks.txt");
        File.WriteAllText(jsonFilePath, json);
    }

    public void RemovePuzzleAtIndex(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < myPuzzleList.puzzlelevel.Count)
        {
            // Remove the element at the specified index
            myPuzzleList.puzzlelevel.RemoveAt(index);

            // Serialize and save the updated data to the JSON file
            SavePuzzleListToJson();
        }
    }
}
