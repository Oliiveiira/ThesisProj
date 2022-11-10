using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCart : MonoBehaviour
{

    [SerializeField]
    private bool startMoving = false;
    [SerializeField]
    private bool isInRange = false;
    private Rigidbody rb;
    public Transform player;
    public Transform cart;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && !startMoving)
            {
                cart.transform.SetParent(player);
                startMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && startMoving)
            {
                cart.transform.parent = null;
                startMoving = false;
            }
            // rb.AddForce(transform.forward, ForceMode.Acceleration);
            //rb.useGravity = true;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            //cart.transform.SetParent(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            //cart.transform.SetParent(cart.transform);
        }
    }

    //// Update is called once per frame
    //void LateUpdate()
    //{
    //    if (startMoving)
    //    {
    //        rb.AddForce(transform.forward, ForceMode.Acceleration);
    //        rb.useGravity = true;
    //    }
    //    else
    //    {
    //        rb.velocity = Vector3.zero;
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        startMoving = true;
    //        cart.transform.SetParent(player);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        startMoving = false;
    //        cart.transform.SetParent(cart.transform);
    //    }
    //}
}
