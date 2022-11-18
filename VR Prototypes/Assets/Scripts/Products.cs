using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using RoboRyanTron.Unite2017.Events;

public class Products : MonoBehaviour
{
    public ObjectSO[] allObjects;
    public TextMeshPro[] productsToGet;
    [SerializeField]
    private GameObject listPaper;
    System.Random random = new();
    public string difficulty = "Easy";
    public int arraySize = 3;
    public GameEvent startTimer;
    private bool alreadyShowed;
    [SerializeField]
    private FloatSO level;

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
        if (!alreadyShowed)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ShowList();
            }
        }
    }

    void ShowList()
    {
        alreadyShowed = true;
        startTimer.Raise();
        listPaper.SetActive(true);
        // int randomIndex = Random.Range(0, allObjects.Length);
        allObjects = allObjects.OrderBy(x => random.Next()).ToArray(); //Randomize Array

        if (level.Value < 10)
        {
            arraySize = 3;
            for (int i = 0; i < arraySize; i++)
            {

                productsToGet[i].text = allObjects[i].productName;

            }

        }
        else if (level.Value >= 10 && level.Value < 25)
        {
            arraySize = 4;
            for (int i = 0; i < arraySize; i++)
            {

                productsToGet[i].text = allObjects[i].productName;

            }
        }
        else if (level.Value >= 25 && level.Value < 50)
        {
            arraySize = 5;
            for (int i = 0; i < arraySize; i++)
            {

                productsToGet[i].text = allObjects[i].productName;

            }
        }
        //Debug.Log(allObjects);
        //switch (difficulty)
        //{
        //    case "Easy":
        //        arraySize = 3;
        //        for (int i = 0; i < arraySize; i++)
        //        {

        //            productsToGet[i].text = allObjects[i].productName;

        //        }
        //        break;
        //    case "Medium":
        //        arraySize = 4;
        //        for (int i = 0; i < arraySize; i++)
        //        {

        //            productsToGet[i].text = allObjects[i].productName;

        //        }
        //        break;
        //    case "High":
        //        arraySize = 5;
        //        for (int i = 0; i < arraySize; i++)
        //        {

        //            productsToGet[i].text = allObjects[i].productName;

        //        }
        //        break;
        //}

        StartCoroutine(HideList());
    }

    private IEnumerator HideList()
    {
        yield return new WaitForSeconds(5);
        listPaper.SetActive(false);
    }


}
