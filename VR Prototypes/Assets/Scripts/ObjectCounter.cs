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
    private GameManager products;
    //[SerializeField]
    //private ObjectSO objectSO;
    private int successCounter;
    [SerializeField]
    private  bool isInFlag;
    [SerializeField]
    private GameEvent scoreUI;
    [SerializeField]
    private GameEvent setWinPanel;

    private void Start()
    {

    }
    private void Update()
    {
        //Win();
    }

    void Win()
    {
        if (successCounter == products.arraySize)
        {
            setWinPanel.Raise();
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
            for(int i = 0; i< products.arraySize; i++)
            {
                if (other.name.Equals(products.productsToGet[i].text))
                {
                    scoreUI.Raise(); //Evento para adicionar 1 ponto no score
                    products.productsToGet[i].SetText("Boa");
                    Debug.Log("yes");
                    successCounter++;
                    isInFlag = true;
                }
            }

            if (!isInFlag)
            {
                Debug.Log("tente outra vez");
                isInFlag = false;
            }

            Win();

        }
    }
}
