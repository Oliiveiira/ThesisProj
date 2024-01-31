using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pairPiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PuzzlePlace"))
        {
            if (Vector3.Distance(transform.position, other.transform.position) <= 0.01f && other.gameObject.name == gameObject.name)
            {
                //Debug.Log("OnTriggerStay Active");
                transform.position = other.transform.position;
                //Debug.Log(other.gameObject.name);
                //placeSound.Play();
                //HandGrabInteractable handGrabInteractable = GetComponent<HandGrabInteractable>();
                //handGrabInteractable.enabled = false;
                //MeshRenderer puzzlePlaceRenderer = other.GetComponent<MeshRenderer>();
                //puzzlePlaceRenderer.enabled = false;
               // DisableInteractableClientRpc(other.gameObject.name);
            }
        }
    }
}
