using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddIngredientToList : LevelsDataWritter
{
    private Button button;
    public TextMeshProUGUI ingredientName;
    public TextMeshProUGUI pathContainers;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void AddIngredientsToRecipe()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        if (myLevelData.levelData[^1].ingredientsPath.Count <= 5)
        {
            // Add the new ingredients to the current recipe
            myLevelData.levelData[^1].ingredientsName.Add(ingredientName.text);
            myLevelData.levelData[^1].ingredientsPath.Add(pathContainers.text);

            //// Serialize and save the updated data to the JSON file
            SaveDataToJson();
            button.interactable = false;
        }
    }
}
