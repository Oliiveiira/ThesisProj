using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class RecipeReader : JSONReader
{
    //public TextAsset recipeJSON;
    public TextMeshPro budgetText;
    public TextMeshPro recipeName;
    public TextMeshPro[] productsToGet;
    [SerializeField]
    private GameObject listPaper;
    public int arraySize = 3;
    public GameEvent startTimer;
    private bool alreadyShowed;
    public int randomIndex;
    //To use in ScanProduct Script
    public int budget;
    [SerializeField]
    private GameObject money;
    public Transform moneyTransform;
    public GameObject icon;

    [SerializeField]
    private IntSO level;

    // Start is called before the first frame update
    void Awake()
    {
        allSprites = Resources.LoadAll<Sprite>("RecipeImages/");
        myRecipeList = JsonUtility.FromJson<RecipeList>(recipeJSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!alreadyShowed)
        //{
        //    if (Input.GetKeyDown(KeyCode.L))
        //    {
        //         ShowRecipe();
        //    }
        //}
        if (!alreadyShowed)
            ShowRecipe();
    }

    private void ShowRecipe()
    {
        alreadyShowed = true;
       // startTimer.Raise();
        listPaper.SetActive(true);

        //its choosing the recepie randomly now, but then its going to be sequentially, based on the difficulty level
        randomIndex = Random.Range(0, myRecipeList.recipe.Length);

        budget = myRecipeList.recipe[level.Value].budget;

        money = (GameObject)Instantiate(Resources.Load(myRecipeList.recipe[level.Value].budgetPrefab), moneyTransform.position, Quaternion.Euler(-90,-90,-90)); //instantiate the money prefab
       // money.transform.position = moneyTransform.position;
        //byte[] bytes = File.ReadAllBytes(myRecipeList.recipe[randomIndex].spriteURL);
        //Texture2D loadTexture = new(1, 1); //mock size 1x1
        //loadTexture.LoadImage(bytes);
        //icon.GetComponent<Renderer>().material.mainTexture = loadTexture;

        icon.GetComponent<SpriteRenderer>().sprite = allSprites[level.Value];

        Debug.Log(myRecipeList.recipe[level.Value].spriteURL);

        budgetText.SetText("Money: " + myRecipeList.recipe[level.Value].budget.ToString());
        recipeName.SetText("Receita: " + myRecipeList.recipe[level.Value].recipeName);

        for(int i=0; i < myRecipeList.recipe[level.Value].ingredients.Length; i++)
        {
            productsToGet[i].text = myRecipeList.recipe[level.Value].ingredients[i];
        }
    }

}
