using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketCounter : MonoBehaviour
{
    //[SerializeField]
    //private ProductListManager productList;
    //[SerializeField]
    //private Products products; //Para utilizar em 3D
    [SerializeField]
    private GameManager products;
    //[SerializeField]
    //private ObjectSO objectSO;
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
        if (successCounter == products.myRecipeList.recipe[products.level.Value].ingredients.Length)
        {
            DeactivateShelves();
            setWinPanel.Raise();
            Debug.Log("Ganhou");
        }
    }

    void DeactivateShelves()
    {
        foreach(GameObject obj in shelves)
        {
            obj.SetActive(false);
        }
    }

    //private ObjectGrabbable grabbedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            //ObjectSO products = other.gameObject.GetComponent<ObjectSO>();
            isInFlag = false;
            for (int i = 0; i < products.myRecipeList.recipe[products.level.Value].ingredients.Length; i++)
            {
                if (other.name.Equals(products.productsToGet[i].text))
                {
                    other.transform.SetParent(this.transform);
                    listProduct.productsToGet[i].SetText("Boa!");
                    //products.productsToGet[i].SetText("Boa");
                    product = other.GetComponent<Product>();
                    product.isInBasket = true;
                    Debug.Log("yes");
                    successCounter++;
                    isInFlag = true;
                    correctSound.Play();
                }
            }

            //if (!isInFlag)
            //{
            //    Debug.Log("tente outra vez");
            //    isInFlag = false;
            //    wrongSound.Play();
            //}

            Win();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            for (int i = 0; i < products.myRecipeList.recipe[products.level.Value].ingredients.Length; i++)
            {
                if (other.name.Equals(products.productsToGet[i].text))
                {
                    other.transform.parent = null;
                    successCounter--;
                }
            }
        }
    }




    //void Win()
    //{
    //    if (successCounter == products.arraySize)
    //    {
    //        setWinPanel.Raise();
    //        Debug.Log("Ganhou");
    //    }
    //}

}
