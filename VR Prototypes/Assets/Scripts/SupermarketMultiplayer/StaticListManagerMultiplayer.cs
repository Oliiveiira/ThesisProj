using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class StaticListManagerMultiplayer : StaticLevelsListReader
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

    [SerializeField]
    private int minLevelValue;
    [SerializeField]
    private int maxLevelValue;

    // Start is called before the first frame update
    void Awake()
    {
        allSprites = Resources.LoadAll<Sprite>("StaticLevelsImages/");
        mystaticLevelsLists = JsonUtility.FromJson<StaticLevelsList>(recipeJSON.text);
    }

    private void Start()
    {
        if (!IsServer)
            return;

        GenerateCode();
        Random random = new Random();
        randomIndex = random.Next(minLevelValue, maxLevelValue + 1);
        level.Value = randomIndex;
        SetLevelClientRPC(randomIndex);
        ShowRecipe();
    }

    [ClientRpc]
    public void SetLevelClientRPC(int randomIndex)
    {
        level.Value = randomIndex;
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
        //startButton.SetActive(false);
        //leftButton.SetActive(false);
        //rightButton.SetActive(false);
        // alreadyShowed = true;
        //startTimer.Raise();
        //listPaper.SetActive(true);
        //if (mirrorLeft)
        //    watchR.SetActive(true);
        //if (mirrorRight)
        //    watchL.SetActive(true);
        //if (!mirrorRight && !mirrorLeft)
        //{
        //    watchL.SetActive(true);
        //}
        //its choosing the recepie randomly now, but then its going to be sequentially, based on the difficulty level
        //randomIndex = Random.Range(0, myRecipeList.recipe.Length);

        //budget = mystaticLevelsLists.recipe[level.Value].budget;

        //image.sprite = allSprites[level.Value];

        //recipeName.SetText(mystaticLevelsLists.recipe[level.Value].recipeName);

        //Debug.Log(mystaticLevelsLists.recipe[level.Value].spriteURL);

        //if (level.Value >= 0 && level.Value <= 2 || level.Value >= 9 && level.Value <= 11)
        //{
        //    budgettoWatchR.SetText("Dinheiro: " + mystaticLevelsLists.recipe[level.Value].budget.ToString() + "€"); //to Watch
        //    budgettoWatchL.SetText("Dinheiro: " + mystaticLevelsLists.recipe[level.Value].budget.ToString() + "€"); //to Watch

        //    budgetText.SetText("Dinheiro: " + mystaticLevelsLists.recipe[level.Value].budget.ToString() + "€");
        //}
        //else if (level.Value >= 3 && level.Value <= 5)
        //{
        //    budgettoWatchR.SetText("Codigo: " + code); //to Watch
        //    budgettoWatchL.SetText("Codigo: " + code); //to Watch

        //    budgetText.SetText("Codigo: " + code);
        //}
        //else if (level.Value >= 6 && level.Value <= 8)
        //{
        //    budgettoWatchR.SetText("MBWay"); //to Watch
        //    budgettoWatchL.SetText("MBWay"); //to Watch

        //    budgetText.SetText("MBWay");
        //}

        for (int i = 0; i < mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
        {
            productsToGet[i].text = mystaticLevelsLists.recipe[level.Value].ingredientsName[i];
            productsToGettoWatchR[i].text = mystaticLevelsLists.recipe[level.Value].ingredientsName[i]; //to Watch
            productsToGettoWatchL[i].text = mystaticLevelsLists.recipe[level.Value].ingredientsName[i]; //to Watch
        }
        SetListClientRPC();
    }

    [ClientRpc]
    public void SetListClientRPC()
    {
        budget = mystaticLevelsLists.recipe[level.Value].budget;

        image.sprite = allSprites[level.Value];

        recipeName.SetText(mystaticLevelsLists.recipe[level.Value].recipeName);

        for (int i = 0; i < mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
        {
            productsToGet[i].text = mystaticLevelsLists.recipe[level.Value].ingredientsName[i];
            productsToGettoWatchR[i].text = mystaticLevelsLists.recipe[level.Value].ingredientsName[i]; //to Watch
            productsToGettoWatchL[i].text = mystaticLevelsLists.recipe[level.Value].ingredientsName[i]; //to Watch
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene(string sceneName)
    {
        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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

}
