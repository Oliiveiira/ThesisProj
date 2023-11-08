using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private IntSO paymentMethod;
    public GameObject tenEurosWallet;
    public GameObject twentyEurosWallet;
    public GameObject cardWallet;
    public GameObject phone;

    // Start is called before the first frame update
    void Start()
    {
        if(paymentMethod.Value == 1)
        {
            tenEurosWallet.SetActive(true);
        }
        else if(paymentMethod.Value == 2)
        {
            twentyEurosWallet.SetActive(true);
        }
        else if (paymentMethod.Value == 3)
        {
            cardWallet.SetActive(true);
        }
        else if (paymentMethod.Value == 4)
        {
            phone.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
