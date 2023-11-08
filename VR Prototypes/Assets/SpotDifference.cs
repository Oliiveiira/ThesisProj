using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotDifference : MonoBehaviour
{
    public GameObject spotCircle;
    public bool isSpotted;
    private AudioSource spottedSound;

    // Start is called before the first frame update
    void Start()
    {
        spottedSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided");
            if (!isSpotted)
            {
                spottedSound.Play();
                spotCircle.SetActive(true);
                DifferencesGameManager.Instance.spottedDifferences.Add(1);
                isSpotted = true;
            }
        }
    }
}
