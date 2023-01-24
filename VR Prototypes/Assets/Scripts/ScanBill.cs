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
            billSound.Play();
            moveMoney = true;
            //other.attachedRigidbody.isKinematic = true;
            //other.attachedRigidbody.useGravity = false;
            //ObjectGrabbable objectGrabbable = other.GetComponent<ObjectGrabbable>();
            //objectGrabbable.Drop();

            grabInteractor.ForceRelease();
            handgrabInteractorR.ForceRelease();
            handgrabInteractorL.ForceRelease();

            Money money = other.GetComponent<Money>();
            total.Value -= money.value;

            a = other.transform.position;
            b = billTransform.position;

            billPosition = other.transform;

            totalCost.SetText("Total: " + total.Value.ToString());
        }
    }
}
