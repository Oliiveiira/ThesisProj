using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject nextArrow;
    public GameObject previousInstruction;
    public GameObject nextInstruction;
    public GameObject nextWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            previousInstruction.SetActive(false);
            nextInstruction.SetActive(true);
            nextArrow.SetActive(true);
            nextWaypoint.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
