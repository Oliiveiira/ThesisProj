using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLevelsBasketCounter : MonoBehaviour
{
    [SerializeField]
    private StaticListManager products;

    private int successCounter;
    [SerializeField]
    private bool isInFlag;
    [SerializeField]
    private GameEvent setWinPanel;

    [SerializeField]
    private AudioSource correctSound;
    [SerializeField]
    private AudioSource wrongSound;

    [SerializeField]
    private GameObject[] shelves;

    [SerializeField]
    private RecipeReader listProduct;

    private Product product;

    [SerializeField]
    private GameObject registerBox;

    //[SerializeField]
    //private GameObject wallet;

    //Flag to stop Comparing the products name, in order to reset the products in the basket in the end of the level
    private bool stopComparing;

    [SerializeField]
    private IntSO level;

    private void Start()
    {

    }
    private void Update()
    {
        // PlayAudio();
        //Win();
    }

    void Win()
    {
        if (successCounter == products.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count)
        {
            DeactivateShelves();
            registerBox.SetActive(true);
           // wallet.SetActive(true);
            stopComparing = true;
            // setWinPanel.Raise();
            Debug.Log("Ganhou");
        }
    }

    void DeactivateShelves()
    {
        foreach (GameObject obj in shelves)
        {
            obj.SetActive(false);
        }
    }

    //private ObjectGrabbable grabbedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (!stopComparing)
        {
            if (other.CompareTag("Objects"))
            {
                //ObjectSO products = other.gameObject.GetComponent<ObjectSO>();
                product = other.GetComponent<Product>();
                isInFlag = false;
                for (int i = 0; i < products.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
                {
                    if (other.name.Equals(products.productsToGet[i].text))
                    {
                        other.transform.SetParent(this.transform);
                        product.isInBasket = true;
                        Debug.Log("yes");
                        successCounter++;
                        isInFlag = true;
                        correctSound.Play();
                        break;
                    }
                }
                if (!product.isInBasket)
                {
                    product.SetProductInitialPosition();
                }

                Win();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!stopComparing)
        {
            if (other.CompareTag("Objects"))
            {
                for (int i = 0; i < products.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
                {
                    if (other.name.Equals(products.productsToGet[i].text))
                    {
                        other.transform.parent = null;
                        successCounter--;
                        product.isInBasket = false;
                    }
                }
            }
        }
    }
}
