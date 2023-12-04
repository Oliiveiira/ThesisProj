using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FillStaticShelf : StaticLevelsListReader
{
    public Transform[] placeHolders;

    public List<GameObject> allProducts;
    public List<GameObject> jsonPrefabs = new List<GameObject>();
    public GameObject placeHoldersObject;

    [SerializeField]
    private IntSO level;

    // Start is called before the first frame update
    void Awake()
    {
        allProducts = new List<GameObject>(Resources.LoadAll<GameObject>("AllProducts/"));
        mystaticLevelsLists = JsonUtility.FromJson<StaticLevelsList>(recipeJSON.text);
        //  myCustomRecipeList = JsonUtility.FromJson<CustomRecipeList>(customRecipeJSON.text);
    }

    private void Start()
    {
        //string jsonFileName = "ProductsList.txt";
        //string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        //// Check if the file exists in the persistent data path
        //string jsonText = File.ReadAllText(jsonFilePath);
        //mystaticLevelsLists = JsonUtility.FromJson<StaticLevelsList>(jsonText);

        //string jsonFilePath = "Assets/Resources/Recipes/ProductsList.txt";
        //string jsonText = File.ReadAllText(jsonFilePath);
        //myProductLists = JsonUtility.FromJson<ProductList>(jsonText);

        //if(customLevel.Value >= 2)
        //{
        //    InstantiatePrefabs();
        //}
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(myProductLists.recipes[0].ingredientsPath);
            //  LoadPrefabsFromJSON();
            InstantiatePrefabs();
        }
    }

    private void LoadPrefabsFromJSON()
    {
        List<string> ingredientPaths = mystaticLevelsLists.recipe[level.Value].ingredientsPath;

        for (int i = 0; i < ingredientPaths.Count; i++)
        {
            GameObject ingredientToCatch = Resources.Load<GameObject>(mystaticLevelsLists.recipe[level.Value].ingredientsPath[i]);
            jsonPrefabs.Add(ingredientToCatch);
        }
        //for (int i = 0; i < myProductLists.recipes[0].ingredientsName.Count; i++)
        //{
        //    Debug.Log("ERRO");
        //    GameObject ingredientToCatch = Resources.Load<GameObject>(myProductLists.recipes[0].ingredientsPath[i]);
        //    jsonPrefabs.Add(ingredientToCatch);
        //}

        //for (int i =0;i< myCustomRecipeList.recipe[0].ingredientName.Length; i++)
        //{
        //    GameObject ingredientToCatch = Resources.Load<GameObject>(myCustomRecipeList.recipe[0].ingredientName[i]);
        //    jsonPrefabs.Add(ingredientToCatch);
        //}
    }

    public void InstantiatePrefabs()
    {
        LoadPrefabsFromJSON();
        allProducts.RemoveAll(item => jsonPrefabs.Contains(item)); //Remove all products that are common in both lists to avoid duplicated items

        //List<int> usedIndices = new List<int>(); // Store the indices that have been used
        //int lastJsonPrefabIndex = 0;
        //for (int i = 0; i < jsonPrefabs.Count; i++)
        //{
        //    if (!usedIndices.Contains(i))
        //    {
        //        Random.seed = System.DateTime.Now.Millisecond;

        //        int randomIndex = Random.Range(0, placeHolders.Length);
        //        while (randomIndex == lastJsonPrefabIndex)
        //        {
        //            randomIndex = Random.Range(0, placeHolders.Length);
        //        }
        //        lastJsonPrefabIndex = randomIndex;
        //        GameObject InstantiatedPrefab = Instantiate(jsonPrefabs[i], placeHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
        //        InstantiatedPrefab.transform.parent = placeHolders[randomIndex].transform;
        //        usedIndices.Add(randomIndex); // Mark the index as used
        //    }
        //}

        List<int> usedIndices = new List<int>(); // Store the indices that have been used
        List<int> lastJsonPrefabIndex = new List<int>();

        for (int i = 0; i < jsonPrefabs.Count; i++)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            int randomIndex = Random.Range(0, placeHolders.Length);
            while (lastJsonPrefabIndex.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, placeHolders.Length);
            }
            lastJsonPrefabIndex.Add(randomIndex);
            // Instantiate(jsonPrefabs[i], placeHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            GameObject InstantiatedPrefab = Instantiate(jsonPrefabs[i], placeHolders[randomIndex].position, jsonPrefabs[i].transform.rotation);
            InstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            usedIndices.Add(randomIndex); // Mark the index as used
        }

        List<int> lastIndex = new List<int>();
        for (int j = 0; j < placeHolders.Length; j++)
        {
            if (!usedIndices.Contains(j))
            {
                Random.seed = System.DateTime.Now.Millisecond;

                int randomIndex = Random.Range(0, allProducts.Count - 1);
                while (lastIndex.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, allProducts.Count - 1);
                }
                lastIndex.Add(randomIndex);
                // Instantiate(allProducts[randomIndex], placeHolders[j].position, allProducts[randomIndex].transform.rotation);
                GameObject InstantiatedPrefab = Instantiate(allProducts[randomIndex], placeHolders[j].position, allProducts[randomIndex].transform.rotation);
                InstantiatedPrefab.transform.parent = placeHoldersObject.transform;
            }

        }
    }
}
