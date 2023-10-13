using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRecipeReader : MonoBehaviour
{
    public TextAsset customRecipeJSON;
    // public Recipe ingredients;

    [System.Serializable]
    public class CustomRecipe
    {
        public string[] ingredientName;
    }

    [System.Serializable]
    public class CustomRecipeList
    {
        public List<CustomRecipe> recipe;
    }

    public CustomRecipeList myCustomRecipeList = new CustomRecipeList();
}
