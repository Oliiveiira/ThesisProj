using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCart : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        //rb.detectCollisions = false;
        //rb.isKinematic = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        rb.isKinematic = false;
    //        rb.detectCollisions = true;
    //        Debug.Log("Funcionou!");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        rb.isKinematic = true;
    //        rb.detectCollisions = false;
    //    }
    //}
}
