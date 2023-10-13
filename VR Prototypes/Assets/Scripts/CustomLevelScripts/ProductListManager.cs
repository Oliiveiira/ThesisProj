using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.SceneManagement;
using System.IO;

public class ProductListManager : ProductListReader
{

    //public TextAsset recipeJSON;
    //public TextMeshProUGUI budgetText;
    //public TextMeshProUGUI recipeName;
    public TextMeshProUGUI[] productsToGet;
    [SerializeField]
    private GameObject listPaper;
    public int arraySize = 3;
    public GameEvent startTimer;

    //To use in ScanProduct Script
    //public int budget;
    //[SerializeField]
    //private GameObject money;
    //public Transform moneyTransform;
    //[SerializeField]
    //private UnityEngine.UI.Image image = null;

    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject leftButton;
    [SerializeField]
    private GameObject rightButton;

    //To control the Data Watch
  //  public TextMeshProUGUI budgettoWatchR;
    public TextMeshProUGUI[] productsToGettoWatchR;
    [SerializeField]
    private GameObject watchR;

  //  public TextMeshProUGUI budgettoWatchL;
    public TextMeshProUGUI[] productsToGettoWatchL;
    [SerializeField]
    private GameObject watchL;

    public bool mirrorLeft;
    public bool mirrorRight;

    public IntSO level;

    // Start is called before the first frame update
    void Awake()
    {
       // myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);
    }

    private void Start()
    {
        // Load the JSON data from the file every time the scene starts
        string jsonFilePath = "Assets/Resources/Recipes/ProductsList.txt";
        string jsonText = File.ReadAllText(jsonFilePath);
        //Debug.Log(jsonText);
        myProductLists = JsonUtility.FromJson<ProductList>(jsonText);

      //  myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);
    }
    // Update is called once per frame
    void Update()
    {
        //To test w/o the HMD
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    for (int i = 0; i < myProductLists.recipes.Count; i++)
        //    {
        //        productsToGet[i].text = myProductLists.recipes[0].ingredientsName[i];
        //        productsToGettoWatchR[i].text = myProductLists.recipes[0].ingredientsName[i];  //to Watch
        //        productsToGettoWatchL[i].text = myProductLists.recipes[0].ingredientsName[i]; //to Watch
        //    }
        //}
    }

    public void ShowRecipe()
    {
     //   myProductLists = JsonUtility.FromJson<ProductList>(productListJSON.text);

        startButton.SetActive(false);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        // alreadyShowed = true;
        startTimer.Raise();
        listPaper.SetActive(true);
        if (mirrorLeft)
            watchR.SetActive(true);
        if (mirrorRight)
            watchL.SetActive(true);
        if (!mirrorRight && !mirrorLeft)
        {
            watchL.SetActive(true);
        }

        //budget = myRecipeList.recipe[level.Value].budget;

        //money = (GameObject)Instantiate(Resources.Load(myRecipeList.recipe[level.Value].budgetPrefab)); //instantiate the money prefab
        //money.transform.position = moneyTransform.position;

        //image.sprite = allSprites[level.Value];

        //Debug.Log(myRecipeList.recipe[level.Value].spriteURL);

        //budgettoWatchR.SetText("Dinheiro: " + myRecipeList.recipe[level.Value].budget.ToString() + "€"); //to Watch
        //budgettoWatchL.SetText("Dinheiro: " + myRecipeList.recipe[level.Value].budget.ToString() + "€"); //to Watch

        //budgetText.SetText("Dinheiro: " + myRecipeList.recipe[level.Value].budget.ToString() + "€");
        //recipeName.SetText(myRecipeList.recipe[level.Value].recipeName);

        for (int i = 0; i < myProductLists.recipes[0].ingredientsName.Count; i++)
        {
            productsToGet[i].text = myProductLists.recipes[0].ingredientsName[i];
            productsToGettoWatchR[i].text = myProductLists.recipes[0].ingredientsName[i];  //to Watch
            productsToGettoWatchL[i].text = myProductLists.recipes[0].ingredientsName[i]; //to Watch
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
