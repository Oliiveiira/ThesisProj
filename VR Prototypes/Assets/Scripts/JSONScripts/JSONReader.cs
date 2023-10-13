using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public Sprite[] allSprites;
    public TextAsset recipeJSON;
   // public Recipe ingredients;

    [System.Serializable]
    public class Recipe
    {
        public int budget;
        public string budgetPrefab;
        public string spriteURL;
        public string recipeName;
        public string[] ingredients;
    }

    [System.Serializable]
    public class RecipeList
    {
        public Recipe[] recipe;
    }

    public RecipeList myRecipeList = new RecipeList();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
