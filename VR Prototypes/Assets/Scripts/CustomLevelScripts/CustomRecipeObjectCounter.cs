using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class CustomRecipeObjectCounter : MonoBehaviour
{

    [SerializeField]
    private ProductListManager list;
    private int successCounter;
    [SerializeField]
    private bool isInFlag;
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
        if (successCounter == list.myProductLists.recipes[0].ingredientsName.Count)
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
            for (int i = 0; i < list.myProductLists.recipes[0].ingredientsName.Count; i++)
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
            if (!product.isInCart)
            {
                product.SetProductInitialPosition();
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
}
