using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RoboRyanTron.Unite2017.Events;

public class ScanProduct : MonoBehaviour
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
    private GameManager ingredients;

    [SerializeField]
    private AudioSource scanSound;//trigger the sound

    private int successCounter;

    private bool isInFlag;
 
    public bool paymentAvailable;
    
    [SerializeField]
    private GameEvent setWinPanel;

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
        if (successCounter == ingredients.myRecipeList.recipe[ingredients.level.Value].ingredients.Length)
        {
            warning.gameObject.SetActive(false);
            paymentAvailable = true;
            //setWinPanel.Raise();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            isInFlag = false;
            for (int i = 0; i < ingredients.myRecipeList.recipe[ingredients.level.Value].ingredients.Length; i++)
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
