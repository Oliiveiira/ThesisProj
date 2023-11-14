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
    }
    public void Set20EurosPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 2;
    }
    public void SetCardPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 3;
    }
    public void SetMBWayPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 4;
    }
    public void SetRaiseMoneyPaymentMethod()
    {
        productList.myProductLists.recipes[recipeNumberSO.Value].paymentMethod = 5;
    }
}
