using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGameObject : MonoBehaviour
{
    private SnapInteractor billInteractor;

    // Start is called before the first frame update
    void Start()
    {
        billInteractor = GetComponentInParent<SnapInteractor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wallet"))
        {
            SnapInteractable walletSnap = other.GetComponent<SnapInteractable>();
            billInteractor._defaultInteractable = walletSnap;
            billInteractor._timeOutInteractable = walletSnap;
        }
    }
}