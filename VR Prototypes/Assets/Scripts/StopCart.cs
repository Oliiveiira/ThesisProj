using Oculus.Interaction.Grab;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cart"))
        {
           // IHandGrabbable cart = other.GetComponent<IHandGrabbable>();
            HandGrabInteractable cart = other.GetComponent<HandGrabInteractable>();
            cart.enabled = false;
        }
    }
}
