using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCounter : MonoBehaviour
{
    //[SerializeField]
    //private ProductListManager productList;
    [SerializeField]
    private Products products;
    //[SerializeField]
    //private ObjectSO objectSO;
    private int successCounter;

    private void Update()
    {
        Win();
    }

    void Win()
    {
        if (successCounter == products.arraySize)
        {
            Debug.Log("Ganhou");
        }
    }

    //private ObjectGrabbable grabbedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            //ObjectSO products = other.gameObject.GetComponent<ObjectSO>();

            for(int i = 0; i< products.arraySize; i++)
            {
                //Debug.Log(other.name);
                if (other.name.Equals(products.productsToGet[i].text))
                {
                    products.productsToGet[i].SetText("Boa");
                    Debug.Log("yes");
                    successCounter++;
                }
                else
                {
                    Debug.Log("tente outra vez");
                }
            }

        }
    }

 
}
