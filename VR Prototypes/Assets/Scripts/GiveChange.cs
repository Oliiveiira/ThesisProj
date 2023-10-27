using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class GiveChange : MonoBehaviour
{
    [SerializeField]
    private FloatSO total;
    [SerializeField]
    private GameObject[] allMoney;
    private GameObject money;
    //public Transform[] changeTransform;
    public List<Transform> changeTransform;

    [SerializeField]
    private GameEvent setWinPanel;

    private List<GameObject> moneyObjects = new List<GameObject>();
    public TMP_Text totalCost;
    //[SerializeField]
    //private TMP_Text change;
    private bool setChange = false;
    private float totalPositive;

    public GameObject cashRegister;
    public float probability;
    public bool giveWrongChange;
    public bool resetChange;


    // Start is called before the first frame update
    void Start()
    {
        allMoney = Resources.LoadAll<GameObject>("Money/");

        //// Set the seed based on the current system time
        //UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

        //probability = UnityEngine.Random.value;
        //if (probability < 0.5)
        //    giveWrongChange = true;
        //else
        //    giveWrongChange = false;
    }

    public void Win()
    {
        //Before this win, it is needed to confirm if the change is effectively well
        if (!giveWrongChange)
        {
            cashRegister.SetActive(false);
            setWinPanel.Raise();
        }
    }

    public void WrongChange()
    {
        //Give another change, the idea is to have a random feature which can give the right and the wrong change in a random way, following a probability
        for (int i = 0; i < changeTransform.Count; i++)
        {
            if (changeTransform[i].childCount > 0)
            {
                Destroy(changeTransform[i].GetChild(0).gameObject); // Destroy each child game object
            }
        }
        moneyObjects.Clear(); // Clear the list if it exceeds available positions
        total.Value = totalPositive * -1; // Reset the total value to its original positive value
    }

    public void GiveRandomChange()
    {
        if (total.Value < 0)
        {
            if (!setChange)
            {
                totalPositive = total.Value * -1;
                //change.SetText("Troco: " + totalpositive.ToString());
                totalCost.SetText("Troco: " + totalPositive.ToString());
                setChange = true;
            }

            // Set the seed based on the current system time
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            probability = UnityEngine.Random.value;
            if (probability < 0.5)
                giveWrongChange = true;
            else
                giveWrongChange = false;

            // giveWrongChange = UnityEngine.Random.value < 0.5f; // 50% probability for wrong change

            //  List<GameObject> moneyObjects = new List<GameObject>();

            if (!giveWrongChange)
            {
                while (total.Value < 0)
                {
                    // Randomly select a money object
                    GameObject randomMoneyObject = allMoney[UnityEngine.Random.Range(0, allMoney.Length)];

                    Money moneyScript = randomMoneyObject.GetComponent<Money>();

                    if (moneyScript != null && moneyScript.value <= -total.Value && total.Value <= 0)
                    {
                        moneyObjects.Add(randomMoneyObject);

                        // Deduct the money's value from the total
                        total.Value += moneyScript.value;
                        Debug.Log(moneyScript.value);
                    }
                    //else
                    //{
                    //    Debug.Log("Exited Loop");
                    //    // If no suitable money object is found, exit the loop to avoid infinite looping
                    //    continue;
                    //}
                }
                // bool rightValues;
                if (moneyObjects.Count > changeTransform.Count)
                {
                    moneyObjects.Clear(); // Clear the list if it exceeds available positions
                    total.Value = totalPositive * -1; // Reset the total value to its original positive value
                    Debug.Log(totalPositive);
                }
            }
            else
            {
                while (total.Value < -1)
                {
                    // Randomly select a money object
                    GameObject randomMoneyObject = allMoney[UnityEngine.Random.Range(0, allMoney.Length)];

                    Money moneyScript = randomMoneyObject.GetComponent<Money>();

                    if (moneyScript != null && moneyScript.value <= -total.Value && total.Value <= 0)
                    {
                        moneyObjects.Add(randomMoneyObject);

                        // Deduct the money's value from the total
                        total.Value += moneyScript.value;
                        Debug.Log(moneyScript.value);
                    }
                    //else
                    //{
                    //    Debug.Log("Exited Loop");
                    //    // If no suitable money object is found, exit the loop to avoid infinite looping
                    //    continue;
                    //}
                }
                // bool rightValues;
                if (moneyObjects.Count > changeTransform.Count)
                {
                    moneyObjects.Clear(); // Clear the list if it exceeds available positions
                    total.Value = totalPositive * -1; // Reset the total value to its original positive value
                    Debug.Log(totalPositive);
                }
                else if (moneyObjects.Count <= changeTransform.Count)
                {
                    total.Value = 0;
                }
            }
           

            moneyObjects.Sort((a, b) => a.GetComponent<Money>().value.CompareTo(b.GetComponent<Money>().value));
            for (int i = 0; i < moneyObjects.Count && i < changeTransform.Count; i++)
                {
                    if (moneyObjects[i].CompareTag("Bills"))
                    {
                        GameObject instantiatedPrefab = Instantiate(moneyObjects[i], changeTransform[i].position, Quaternion.Euler(-90, -90, -90));
                        instantiatedPrefab.transform.parent = changeTransform[i].transform;
                       // changeTransform.RemoveAt(i);

                    }
                    else if (moneyObjects[i].CompareTag("Coins"))
                    {
                        GameObject instantiatedPrefab = Instantiate(moneyObjects[i], changeTransform[i].position, Quaternion.Euler(0, -90, 180));
                        instantiatedPrefab.transform.parent = changeTransform[i].transform;
                        //changeTransform.RemoveAt(i);
                    }
                }
        }
    }

    void Update()
    {
        //if (resetChange)
        //{
        //    for (int i = 0; i < changeTransform.Count; i++)
        //    {
        //        if (changeTransform[i].childCount > 0)
        //        {
        //            Destroy(changeTransform[i].GetChild(0).gameObject); // Destroy each child game object
        //        }
        //    }
        //    moneyObjects.Clear(); // Clear the list if it exceeds available positions
        //    total.Value = totalPositive * -1; // Reset the total value to its original positive value
        //    resetChange = false;
        //}
        GiveRandomChange();
    }
}

////// Sort the money objects by their value
//moneyObjects.Sort((a, b) => a.GetComponent<Money>().value.CompareTo(b.GetComponent<Money>().value));


//// Instantiate the sorted money objects at the changeTransform positions
//for (int i = 0; i < moneyObjects.Count && i < changeTransform.Count; i++)
//{
//    if (moneyObjects[i].CompareTag("Bills"))
//    {
//        Instantiate(moneyObjects[i], changeTransform[i].position, Quaternion.Euler(-90, -90, 0));
//        changeTransform.RemoveAt(i);

//    }else if (moneyObjects[i].CompareTag("Coins"))
//    {
//        Instantiate(moneyObjects[i], changeTransform[i].position, Quaternion.Euler(0, 0, 180));
//        changeTransform.RemoveAt(i);
//    }
// Debug.Log("Instantiating: " + moneyObjects[i].name + " at position: " + changeTransform[i].position);

// }