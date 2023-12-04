using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticLevelsScanProduct : MonoBehaviour
{
    [SerializeField]
    private TMP_Text totalCost;
    [SerializeField]
    private TMP_Text warning;
    //  private float total;
    [SerializeField]
    private FloatSO total;
    [SerializeField]
    private Transform basketTransform;
    [SerializeField]
    //private RecipeReader ingredients; //Para utilizar em 3D
    private StaticListManager ingredients;

    [SerializeField]
    private AudioSource scanSound;//trigger the sound
    [SerializeField]
    private AudioSource introduceMoney;//trigger the sound
    [SerializeField]
    private AudioSource introduceCard;//trigger the sound
    [SerializeField]
    private AudioSource scanQRCode;//trigger the sound

    private int successCounter;

    private bool isInFlag;

    public bool paymentAvailable;

    [SerializeField]
    private GameEvent setWinPanel;

    public GameObject qRCode;
    public GameObject wallet;

    //[SerializeField]
    //private IntSO paymentMethod;

    [SerializeField]
    private IntSO level;

    // Start is called before the first frame update
    void Start()
    {
        total.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if(total.Value > ingredients.budget)
        //{
        //    warning.gameObject.SetActive(true);
        //}
    }

    void AllProductsScan()
    {
        if (successCounter == ingredients.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count)
        {
            warning.gameObject.SetActive(false);
            qRCode.SetActive(true);
            wallet.SetActive(true);
            paymentAvailable = true;
            //if (paymentMethod.Value == 1 || paymentMethod.Value == 2)
            //{
            //    introduceMoney.Play();
            //}
            //else if (paymentMethod.Value == 3)
            //{
            //    introduceCard.Play();
            //}
            //else if (paymentMethod.Value == 4)
            //{
            //    scanQRCode.Play();
            //}
            //setWinPanel.Raise();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            isInFlag = false;
            for (int i = 0; i < ingredients.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
            {
                if (other.name.Equals(ingredients.productsToGet[i].text))
                {
                    scanSound.Play();
                    successCounter++;
                    //ObjectGrabbable objectGrabbable = other.GetComponent<ObjectGrabbable>();
                    //objectGrabbable.Drop(); //just for 3d purpose

                    Product product = other.GetComponent<Product>();

                    total.Value += product.productCost; //add the products cost to total

                    totalCost.SetText("Total: " + total.Value.ToString());

                    other.transform.position = basketTransform.position;
                    isInFlag = true;
                    ingredients.productsToGet[i].SetText("Boa!");
                    ingredients.productsToGettoWatchL[i].SetText("Boa!");
                    ingredients.productsToGettoWatchR[i].SetText("Boa!");
                }
            }

            if (!isInFlag)
            {
                Debug.Log("Produto errado..");
                isInFlag = false;
            }

            AllProductsScan();
        }
    }
}
