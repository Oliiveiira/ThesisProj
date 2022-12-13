using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    [SerializeField]
    private ObjectSO objects; //ScriptableObject with the product data
    public float productCost;

    // Start is called before the first frame update
    private void Awake()
    {
        name = objects.productName;
        productCost = objects.productCost;
    }
}
