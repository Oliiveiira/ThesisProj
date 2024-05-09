using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AddRecipeNameMultiplayer : LevelsDataWritter
{
    public TextMeshProUGUI recipe;

    private void Start()
    {

    }

    public void RecipeNameAdd()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        myLevelData.levelData[^1].recipeName = recipe.text;
        SaveDataToJson();
    }
}
