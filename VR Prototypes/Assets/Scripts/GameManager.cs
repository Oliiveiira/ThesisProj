using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Events;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public ObjectSO[] allObjects;
    public TextMeshProUGUI[] productsToGet;
    [SerializeField]
    private GameObject list;
    System.Random random = new();
    public string difficulty = "Easy";
    public int arraySize = 3;
    public GameEvent startTimer;
    [SerializeField]
    private FloatSO level;
    [SerializeField]
    private GameObject startButton;

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

    }

    public void ShowList()
    {
        startButton.SetActive(false);
        startTimer.Raise();
        list.SetActive(true);
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
        StartCoroutine(HideList());
    }

    private IEnumerator HideList()
    {
        yield return new WaitForSeconds(5);
        list.SetActive(false);
    }
}
