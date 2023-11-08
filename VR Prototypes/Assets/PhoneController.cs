using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject finalScreen;
    public CustomScanproduct paymentAvailable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("QRCode"))
        {
            if(paymentAvailable)
                finalScreen.SetActive(true);
        }
    }
}
