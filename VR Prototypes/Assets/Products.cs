using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Products : MonoBehaviour
{
    public ObjectSO[] allObjects;
    public TextMeshPro[] productsToGet;
    [SerializeField]
    private GameObject listPaper;
    System.Random random = new();
    public string difficulty = "Easy";
    public int arraySize = 3;

    private void Awake()
    {
        allObjects = Resources.LoadAll<ObjectSO>("SupermarketProducts/");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowList();
        }
    }

    void ShowList()
    {
        listPaper.SetActive(true);
        // int randomIndex = Random.Range(0, allObjects.Length);
        allObjects = allObjects.OrderBy(x => random.Next()).ToArray(); //Randomize Array

        //Debug.Log(allObjects);
        switch (difficulty)
        {
            case "Easy":
                arraySize = 3;
                for (int i = 0; i < arraySize; i++)
                {

                    productsToGet[i].text = allObjects[i].objectName;

                }
                break;
            case "Medium":
                arraySize = 4;
                for (int i = 0; i < arraySize; i++)
                {

                    productsToGet[i].text = allObjects[i].objectName;

                }
                break;
            case "High":
                arraySize = 5;
                for (int i = 0; i < arraySize; i++)
                {

                    productsToGet[i].text = allObjects[i].objectName;

                }
                break;
        }
        //if(difficulty == "Easy")
        //{
        //    arraySize = 3;
        //    for (int i = 0; i < arraySize; i++)
        //    {

        //        productsToGet[i].text = allObjects[i].objectName;

        //    }
        //}else if (difficulty == "Medium")
        //{
        //    arraySize = 4;
        //    for (int i = 0; i < arraySize; i++)
        //    {

        //        productsToGet[i].text = allObjects[i].objectName;

        //    }
        //}
        //else if (difficulty == "High")
        //{
        //    arraySize = 5;
        //    for (int i = 0; i < arraySize; i++)
        //    {

        //        productsToGet[i].text = allObjects[i].objectName;

        //    }
        //}

    }


}
