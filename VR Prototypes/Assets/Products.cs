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

    void Difficulty()
    {
        if(difficulty == "Easy")
        {
            productsToGet.Length.Equals(3);
        }
    }

    void ShowList()
    {
        Difficulty();
        listPaper.SetActive(true);
        // int randomIndex = Random.Range(0, allObjects.Length);
        allObjects = allObjects.OrderBy(x => random.Next()).ToArray(); //Randomize Array
        Debug.Log(allObjects);
        for (int i = 0; i < productsToGet.Length; i++)
        {

            productsToGet[i].text = allObjects[i].objectName;

        }
    }


}
