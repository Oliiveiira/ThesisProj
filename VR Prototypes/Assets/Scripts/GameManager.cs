using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Events;
using System.Linq;
using UnityEngine.SceneManagement;

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


    // Start is called before the first frame update
    void Awake()
    {
        allSprites = Resources.LoadAll<Sprite>("RecipeImages/");
        myRecipeList = JsonUtility.FromJson<RecipeList>(recipeJSON.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowRecipe()
    {
        startButton.SetActive(false);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        // alreadyShowed = true;
        //startTimer.Raise();
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
        randomIndex = Random.Range(0, myRecipeList.recipe.Length);

        budget = myRecipeList.recipe[randomIndex].budget;

        money = (GameObject)Instantiate(Resources.Load(myRecipeList.recipe[randomIndex].budgetPrefab)); //instantiate the money prefab
        money.transform.position = moneyTransform.position;

        image.sprite = allSprites[randomIndex];

        Debug.Log(myRecipeList.recipe[randomIndex].spriteURL);

        budgettoWatchR.SetText("Money: " + myRecipeList.recipe[randomIndex].budget.ToString()); //to Watch
        budgettoWatchL.SetText("Money: " + myRecipeList.recipe[randomIndex].budget.ToString()); //to Watch

        budgetText.SetText("Money: " + myRecipeList.recipe[randomIndex].budget.ToString());
        recipeName.SetText(myRecipeList.recipe[randomIndex].recipeName);

        for (int i = 0; i < myRecipeList.recipe[randomIndex].ingredients.Length; i++)
        {
            productsToGet[i].text = myRecipeList.recipe[randomIndex].ingredients[i];
            productsToGettoWatchR[i].text = myRecipeList.recipe[randomIndex].ingredients[i]; //to Watch
            productsToGettoWatchL[i].text = myRecipeList.recipe[randomIndex].ingredients[i]; //to Watch
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
