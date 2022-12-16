using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scanMoney : MonoBehaviour
{
    private bool isInFlag;
    public Transform moneyTransform;
    [SerializeField]
    private TMP_Text totalCost;
    [SerializeField]
    private TMP_Text change;
    [SerializeField]
    private ScanProduct products; //to get the products total
    [SerializeField]
    private FloatSO total;

    [SerializeField]
    private float totalpositive;//to give change

    [SerializeField]
    private GrabInteractor grabInteractor;//to give change

    [SerializeField]
    private bool moveMoney;
    [SerializeField]
    private Transform moneyPosition;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(total.Value <= 0)
        {
            totalpositive = total.Value * -1;
            change.SetText("Troco: " + totalpositive.ToString());
            totalCost.SetText("Total: 0");
            //total.Value = 0;
            //totalCost.SetText("Total: " + total.Value.ToString());
        }
    }

    private void FixedUpdate()
    {
        if (moveMoney)
        {
            speed = (float)(speed + 0.002);
            moneyPosition.position = Vector3.MoveTowards(a, b, speed);

            if(moneyPosition.position == b)
            {
                moveMoney = false;
                speed = (float) 0.2;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            moveMoney = true;

            ObjectGrabbable objectGrabbable = other.GetComponent<ObjectGrabbable>();
            objectGrabbable.Drop();

            grabInteractor.ForceRelease();

            Money money = other.GetComponent<Money>();
            total.Value -= money.value;

            a = other.transform.position;
            b = moneyTransform.position;

            moneyPosition = other.transform;

            totalCost.SetText("Total: " + total.Value.ToString());
        }
    }
}
