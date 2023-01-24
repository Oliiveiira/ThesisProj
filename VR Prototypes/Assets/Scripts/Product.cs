using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Product : MonoBehaviour
{
    [SerializeField]
    private ObjectSO objects; //ScriptableObject with the product data
    public float productCost;
    [SerializeField]
    private TMP_Text costText;

    // Start is called before the first frame update
    private void Awake()
    {
        name = objects.productName;
        productCost = objects.productCost;
        costText.SetText(objects.productCost.ToString() + "€");
    }
}
