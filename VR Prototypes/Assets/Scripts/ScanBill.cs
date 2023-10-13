using Oculus.Interaction.HandGrab;
using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBill : scanMoney
{
    [SerializeField]
    private bool moveMoney;
    [SerializeField]
    private Transform billPosition;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;

    public Transform billTransform;
    public AudioSource billSound;//trigger the sound
    public float speed;
    [SerializeField]
    private GameEvent setWinPanel;

    private void FixedUpdate()
    {
        if (moveMoney)
        {
            speed = (float)(speed + 0.002);
            billPosition.position = Vector3.MoveTowards(a, b, speed);

            if (billPosition.position == b)
            {
                moveMoney = false;
                speed = (float)0.2;
                speed = 0;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bills") /*&& products.paymentAvailable*/)
        {
            if (products.paymentAvailable)
            {
                billSound.Play();
                moveMoney = true;

                HandGrabInteractable billHandGrab = other.GetComponent<HandGrabInteractable>();
                billHandGrab.enabled = false;

                Money money = other.GetComponent<Money>();
                total.Value -= money.value;

                a = other.transform.position;
                b = billTransform.position;

                billPosition = other.transform;

                other.transform.parent = transform;
                
                totalCost.SetText("Total: " + total.Value.ToString());
                setWinPanel.Raise();
            }
            else
            {
                warning.gameObject.SetActive(true);
            }

        }
    }
}
