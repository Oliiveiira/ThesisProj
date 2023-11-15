using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPaymentMethod : ProductListReader
{
    [SerializeField]
    private ProductListWritter productList;
    [SerializeField]
    private IntSO recipeNumberSO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set10EurosPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 1;
        productList.SaveProductListToJson();
    }
    public void Set20EurosPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 2;
        productList.SaveProductListToJson();
    }
    public void SetCardPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 3;
        productList.SaveProductListToJson();
    }
    public void SetMBWayPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 4;
        productList.SaveProductListToJson();
    }
    public void SetRaiseMoneyPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 5;
        productList.SaveProductListToJson();
    }
}
