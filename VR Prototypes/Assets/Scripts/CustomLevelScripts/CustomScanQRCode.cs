using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomScanQRCode : MonoBehaviour
{

    [SerializeField]
    private GameEvent setWinPanel;

    public GameObject cashRegister;

    public CustomScanproduct products; //to get the products total
    public TMP_Text warning;

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
        if (other.CompareTag("Phone") /*&& products.paymentAvailable*/)
        {
            if (products.paymentAvailable)
            {
                cashRegister.SetActive(false);
                setWinPanel.Raise();
            }
            else
            {
                warning.gameObject.SetActive(true);
            }
        }
    }
}
