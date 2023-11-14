using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Events;
using System.Linq;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : JSONReader
{

    //public TextAsset recipeJSON;
    public TextMeshProUGUI budgetText;
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI[] productsToGet;
    [SerializeField]
    private GameObject listPaper;
    public int arraySize = 3;
    public GameEvent startTimer;
    //private bool alreadyShowed;
    public int randomIndex;
    //To use in ScanProduct Script
    public int budget;
    [SerializeField]
    private GameObject money;
    public Transform moneyTransform;
    [SerializeField] 
    private UnityEngine.UI.Image image = null;

    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject leftButton;
    [SerializeField]
    private GameObject rightButton;

    //To control the Data Watch
    public TextMeshProUGUI budgettoWatchR;
    public TextMeshProUGUI[] productsToGettoWatchR;
    [SerializeField]
    private GameObject watchR;

    public TextMeshProUGUI budgettoWatchL;
    public TextMeshProUGUI[] productsToGettoWatchL;
    [SerializeField]
    private GameObject watchL;

    public bool mirrorLeft;
    public bool mirrorRight;

    public IntSO level;

    //creditCard
   // public TextMeshProUGUI uiCode;
    public string code;

    // Start is called before the first frame update
    void Awake()
    {
        allSprites = Resources.LoadAll<Sprite>("RecipeImages/");
        myRecipeList = JsonUtility.FromJson<RecipeList>(recipeJSON.text);
    }

    private void Start()
    {
        GenerateCode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowRecipe();
        }
    }

    public void ShowRecipe()
    {
        startButton.SetActive(false);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        // alreadyShowed = true;
        startTimer.Raise();
        listPaper.SetActive(true);
        if(mirrorLeft)
        watchR.SetActive(true);
        if(mirrorRight)
        watchL.SetActive(true);
        if(!mirrorRight && !mirrorLeft)
        {
            watchL.SetActive(true);
        }
        //its choosing the recepie randomly now, but then its going to be sequentially, based on the difficulty level
        //randomIndex = Random.Range(0, myRecipeList.recipe.Length);

        budget = myRecipeList.recipe[level.Value].budget;

        money = (GameObject)Instantiate(Resources.Load(myRecipeList.recipe[level.Value].budgetPrefab)); //instantiate the money prefab
        money.transform.position = moneyTransform.position;

        image.sprite = allSprites[level.Value];

        Debug.Log(myRecipeList.recipe[level.Value].spriteURL);

        if(level.Value == 1|| level.Value == 2)
        {
            budgettoWatchR.SetText("Dinheiro: " + myRecipeList.recipe[level.Value].budget.ToString() + "€"); //to Watch
            budgettoWatchL.SetText("Dinheiro: " + myRecipeList.recipe[level.Value].budget.ToString() + "€"); //to Watch

            budgetText.SetText("Dinheiro: " + myRecipeList.recipe[level.Value].budget.ToString() + "€");
        }else if(level.Value == 3 || level.Value == 4)
        {
            budgettoWatchR.SetText("Código: " + code); //to Watch
            budgettoWatchL.SetText("Código: " + code); //to Watch

            budgetText.SetText("Código: " + code);
        }

        recipeName.SetText(myRecipeList.recipe[level.Value].recipeName);

        for (int i = 0; i < myRecipeList.recipe[level.Value].ingredients.Length; i++)
        {
            productsToGet[i].text = myRecipeList.recipe[level.Value].ingredients[i];
            productsToGettoWatchR[i].text = myRecipeList.recipe[level.Value].ingredients[i]; //to Watch
            productsToGettoWatchL[i].text = myRecipeList.recipe[level.Value].ingredients[i]; //to Watch
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GenerateCode()
    {
        // Instantiate random number generator 
        Random random = new Random();

        // Print 4 random numbers between 50 and 100 
        for (int i = 1; i <= 4; i++)
            code = code + random.Next(0, 9).ToString();
        Debug.Log(code);
    }

    //public ObjectSO[] allObjects;
    //public TextMeshProUGUI[] productsToGet;
    //[SerializeField]
    //private GameObject list;
    //System.Random random = new();
    //public string difficulty = "Easy";
    //public int arraySize = 3;
    //public GameEvent startTimer;
    //[SerializeField]
    //private FloatSO level;
    //[SerializeField]
    //private GameObject startButton;

    //private void Awake()
    //{
    //    allObjects = Resources.LoadAll<ObjectSO>("SupermarketProducts/");
    //}

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //public void ShowList()
    //{
    //    startButton.SetActive(false);
    //    startTimer.Raise();
    //    list.SetActive(true);
    //    allObjects = allObjects.OrderBy(x => random.Next()).ToArray(); //Randomize Array

    //    if (level.Value < 10)
    //    {
    //        arraySize = 3;
    //        for (int i = 0; i < arraySize; i++)
    //        {

    //            productsToGet[i].text = allObjects[i].productName;

    //        }

    //    }
    //    else if (level.Value >= 10 && level.Value < 25)
    //    {
    //        arraySize = 4;
    //        for (int i = 0; i < arraySize; i++)
    //        {

    //            productsToGet[i].text = allObjects[i].productName;

    //        }
    //    }
    //    else if (level.Value >= 25 && level.Value < 50)
    //    {
    //        arraySize = 5;
    //        for (int i = 0; i < arraySize; i++)
    //        {

    //            productsToGet[i].text = allObjects[i].productName;

    //        }
    //    }
    //    StartCoroutine(HideList());
    //}

    //private IEnumerator HideList()
    //{
    //    yield return new WaitForSeconds(5);
    //    list.SetActive(false);
    //}
}
