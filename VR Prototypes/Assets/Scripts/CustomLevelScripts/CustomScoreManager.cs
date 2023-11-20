using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomScoreManager : ProductListReader
{
    [SerializeField]
    private IntSO recipeNumberSO;

    [SerializeField]
    private IntSO setPaymentMethod;

    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        string jsonFileName = "ProductsList.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // Check if the file exists in the persistent data path
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            myProductLists = JsonUtility.FromJson<ProductList>(jsonText);
            Debug.Log("File Exists");
        }
        else
        {
            // Handle the case when the file doesn't exist in the persistent data path
            Debug.LogError("JSON file not found in the persistent data path.");
        }
        //string jsonFilePath = "Assets/Resources/Recipes/ProductsList.txt";
        //string jsonText = File.ReadAllText(jsonFilePath);
        //myProductLists = JsonUtility.FromJson<ProductList>(jsonText);

        if (myProductLists.recipes[recipeNumberSO.Value].paymentMethod == 1)
        {
            setPaymentMethod.Value = 1;
        }
        if (myProductLists.recipes[recipeNumberSO.Value].paymentMethod == 2)
        {
            setPaymentMethod.Value = 2;
        }
        if (myProductLists.recipes[recipeNumberSO.Value].paymentMethod == 3)
        {
            setPaymentMethod.Value = 3;
        }
        if (myProductLists.recipes[recipeNumberSO.Value].paymentMethod == 4)
        {
            setPaymentMethod.Value = 4;
        }
        if (myProductLists.recipes[recipeNumberSO.Value].paymentMethod == 5)
        {
           // setPaymentMethod.Value = 5;
           // SceneManager.LoadScene("MBLevel");
        }

        //Scene scene = SceneManager.GetActiveScene();
        //if (scene.name == "CustomLevel - 1")
        //{
        //    customLevel.Value = 1;
        //}
        //else if (scene.name == "CustomLevel - 2Shelf")
        //{
        //    customLevel.Value = 2;
        //}
        //else if (scene.name == "CustomLevel - 4Shelf")
        //{
        //    customLevel.Value = 3;
        //}
        //else if (scene.name == "CustomLevel - 6Shelf")
        //{
        //    customLevel.Value = 4;
        //}
        //else if (scene.name == "CustomLevel - 8Shelf")
        //{
        //    customLevel.Value = 5;
        //}
        //else if (scene.name == "CustomDefinerLevel")
        //{
        //    customLevel.Value = 0;
        //}
    }

    public void SetLevel1()
    {
        sceneName = "CustomLevel - 1";
    }

    public void SetLevel2()
    {
        sceneName = "CustomLevel - 2Shelf";
    }

    public void SetLevel4()
    {
        sceneName = "CustomLevel - 4Shelf";
    }

    public void SetLevel6()
    {
        sceneName = "CustomLevel - 6Shelf";
    }

    public void SetLevel8()
    {
        sceneName = "CustomLevel - 8Shelf";
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene("CustomLevel - 1");
    }

    public void StartGame()
    {
        recipeNumberSO.Value = 0;
        if(myProductLists.recipes[0].paymentMethod == 5)
        {
            SceneManager.LoadScene("MBLevel");
        }
        else
        {
            SceneManager.LoadScene("CustomLevel - 1");
        }
    }

    public void NextLevel()
    {
        recipeNumberSO.Value++;
        if(recipeNumberSO.Value < myProductLists.recipes.Count - 5)
        {
            if (myProductLists.recipes[recipeNumberSO.Value].paymentMethod == 5)
            {
                SceneManager.LoadScene("MBLevel");
            }
            else
            {
                SceneManager.LoadScene("CustomLevel - 1");
            }
        }
        else
        {
            SceneManager.LoadScene("SupermarketMainMenu");
        }
    
    }

    public void GoToSupermarket()
    {
        SceneManager.LoadScene("CustomLevel - 1");
    }

    public void GoToAppMenu()
    {
        SceneManager.LoadScene("AppHub");
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Set PaymentMethod
    public void Set10EurosPayment()
    {
        setPaymentMethod.Value = 1;
        myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 1;
    }   
    public void Set20EurosPayment()
    {
        setPaymentMethod.Value = 2;
        myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 1;
    }    
    public void SetCardPayment()
    {
        setPaymentMethod.Value = 3;
    }    
    public void SetMbWayPayment()
    {
        setPaymentMethod.Value = 4;
    }    
    public void SetRaiseMoneyPayment()
    {
        setPaymentMethod.Value = 5;
        sceneName = "MBLevel";
    }

    public void DeserializeJson()
    {
        string jsonFileName = "ProductsList.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);
        string jsonText = File.ReadAllText(jsonFilePath);
        myProductLists = JsonUtility.FromJson<ProductList>(jsonText);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
