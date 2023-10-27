using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class PuzzlePiece : MonoBehaviour
{
    public Vector3 rightPosition;
    public bool alreadyPlaced;
    // Start is called before the first frame update
    public bool isInRightPlace;
    private AudioSource placeSound;

    private void Awake()
    {
        //rightPosition = transform.position;
    }

    void Start()
    {
        placeSound = GetComponent<AudioSource>();
        // rightPosition = transform.position;
        //transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(0.3f, 0.7f));
        //transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, rightPosition) < 0.035f)
        {
            isInRightPlace = true;
            transform.position = rightPosition;
            transform.rotation = Quaternion.Euler(-90, 0, 0);        
        }
        else
        {
            isInRightPlace = false;
        }
        if (isInRightPlace)
        {
            placeSound.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PuzzlePlace"))
        {
            if (transform.position == other.transform.position)
            {
                Debug.Log(placeSound);
                placeSound.Play();
                HandGrabInteractable handGrabInteractable = GetComponent<HandGrabInteractable>();
                handGrabInteractable.enabled = false;
                MeshRenderer puzzlePlaceRenderer = other.GetComponent<MeshRenderer>();
                puzzlePlaceRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInRightPlace)
        {
            MeshRenderer puzzlePlace = other.GetComponent<MeshRenderer>();
            puzzlePlace.enabled = true;
        }
    }
}
