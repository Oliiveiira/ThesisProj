using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class ObjectCounter : MonoBehaviour
{
    //[SerializeField]
    //private ProductListManager productList;
    //[SerializeField]
    //private Products products; //Para utilizar em 3D
    [SerializeField]
    private GameManager list;
    //[SerializeField]
    //private ObjectSO objectSO;
    private int successCounter;
    [SerializeField]
    private  bool isInFlag;
    [SerializeField]
    private GameEvent scoreUI;
    [SerializeField]
    private GameEvent setWinPanel;

    public bool scanAvaiable; //to ensure that all products are in the cart

    [SerializeField]
    private AudioSource correctSound;
    [SerializeField]
    private AudioSource wrongSound;

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
        if (successCounter == list.myRecipeList.recipe[list.level.Value].ingredients.Length)
        {
            scanAvaiable = true;
            //setWinPanel.Raise();
            Debug.Log("Ganhou");
        }
    }

    //private ObjectGrabbable grabbedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            //ObjectSO products = other.gameObject.GetComponent<ObjectSO>();
            isInFlag = false;
            for (int i = 0; i < list.myRecipeList.recipe[list.level.Value].ingredients.Length; i++)
            {
                if (other.name.Equals(list.productsToGet[i].text))
                {
                    other.transform.SetParent(this.transform);
                    // scoreUI.Raise(); //Evento para adicionar 1 ponto no score
                    //products.productsToGet[i].SetText("Boa");
                    product = other.GetComponent<Product>();
                    if (!product.isInCart)
                    {
                        product.sound.Play();
                        product.isInCart = true;
                    }
                    Debug.Log("yes");
                    successCounter++;
                    isInFlag = true;
                }
            }

            if (!isInFlag)
            {
                Debug.Log("tente outra vez");
                isInFlag = false;
              //  wrongSound.Play();
            }

            Win();

        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Objects"))
    //    {
    //        for (int i = 0; i < products.myRecipeList.recipe[products.level.Value].ingredients.Length; i++)
    //        {
    //            if (other.name.Equals(products.productsToGet[i].text))
    //            {
    //                other.transform.parent = null;
    //                successCounter--;
    //            }
    //        }
    //    }   
    //}
    



    //void Win()
    //{
    //    if (successCounter == products.arraySize)
    //    {
    //        setWinPanel.Raise();
    //        Debug.Log("Ganhou");
    //    }
    //}

    ////private ObjectGrabbable grabbedObject;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Objects"))
    //    {
    //        //ObjectSO products = other.gameObject.GetComponent<ObjectSO>();
    //        isInFlag = false;
    //        for(int i = 0; i< products.arraySize; i++)
    //        {
    //            if (other.name.Equals(products.productsToGet[i].text))
    //            {
    //                scoreUI.Raise(); //Evento para adicionar 1 ponto no score
    //                products.productsToGet[i].SetText("Boa");
    //                Debug.Log("yes");
    //                successCounter++;
    //                isInFlag = true;
    //            }
    //        }

    //        if (!isInFlag)
    //        {
    //            Debug.Log("tente outra vez");
    //            isInFlag = false;
    //        }

    //        Win();

    //    }
    //}
}
