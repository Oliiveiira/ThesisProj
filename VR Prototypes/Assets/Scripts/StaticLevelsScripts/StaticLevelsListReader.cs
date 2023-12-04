using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLevelsListReader : MonoBehaviour
{
    public Sprite[] allSprites;
    public TextAsset recipeJSON;

    [System.Serializable]
    public class Recipe
    {
        public int budget;
        public string recipeName;
        public List<string> ingredientsName;
        public List<string> ingredientsPath;
        public string spriteURL;
        public int paymentMethod;
    }

    [System.Serializable]
    public class StaticLevelsList
    {
        public List<Recipe> recipe;
    }

    public StaticLevelsList mystaticLevelsLists = new StaticLevelsList();

}
