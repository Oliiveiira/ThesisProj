using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductListReader : MonoBehaviour
{
    public TextAsset productListJSON;
    // public Recipe ingredients;

    [System.Serializable]
    public class Recipes
    {
        //public string[] ingredientsName;
        //public string[] ingredientsPath;
        public string recipeName;
        public List<string> ingredientsName;
        public List<string> ingredientsPath;
        public int paymentMethod;
    }

    [System.Serializable]
    public class ProductList
    {
        public List<Recipes> recipes;
    }

    public ProductList myProductLists = new ProductList();

    //[System.Serializable]
    //public class Vegies
    //{
    //    public string[] ingredientsName;
    //    public string[] ingredientsPath;
    //}

    //public class Meals
    //{
    //    public string[] ingredientsName;
    //    public string[] ingredientsPath;
    //    public string category;
    //}

    //public class Breakfast
    //{
    //    public string[] ingredientsName;
    //    public string[] ingredientsPath;
    //}

    //public class Hygiene
    //{
    //    public string[] ingredientsName;
    //    public string[] ingredientsPath;
    //}

    //public class Drinks
    //{
    //    public string[] ingredientsName;
    //    public string[] ingredientsPath;
    //}

    //public class Pasta
    //{
    //    public string[] ingredientsName;
    //    public string[] ingredientsPath;
    //}

    //[System.Serializable]
    //public class ProductList
    //{
    //    public List<Meals> meals;
    //    public List<Drinks> drinks;
    //    public List<Pasta> pastas;
    //    public List<Hygiene> hygiene;
    //    public List<Vegies> vegies;
    //    public List<Breakfast> breakfast;
    //}

    //public ProductList myProductLists = new ProductList();
}
