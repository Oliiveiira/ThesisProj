using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCoin : scanMoney
{
    [SerializeField]
    private bool moveMoney;
    [SerializeField]
    private Transform coinPosition;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;

    public Transform coinTransform;
    public AudioSource coinSound;//trigger the sound
    public float speed;

    private void FixedUpdate()
    {
        if (moveMoney)
        {
            speed = (float)(speed + 0.002);
            coinPosition.position = Vector3.MoveTowards(a, b, speed);

            if (coinPosition.position == b)
            {
                moveMoney = false;
                speed = (float)0.2;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coins"))
        {
            coinSound.Play();
            moveMoney = true;
            other.attachedRigidbody.isKinematic = true;
            ObjectGrabbable objectGrabbable = other.GetComponent<ObjectGrabbable>();
            objectGrabbable.Drop();

            grabInteractor.ForceRelease();
            handgrabInteractorR.ForceRelease();
            handgrabInteractorL.ForceRelease();

            Money money = other.GetComponent<Money>();
            total.Value -= money.value;

            a = other.transform.position;
            b = coinTransform.position;

            coinPosition = other.transform;

            totalCost.SetText("Total: " + total.Value.ToString());
        }
    }
}
