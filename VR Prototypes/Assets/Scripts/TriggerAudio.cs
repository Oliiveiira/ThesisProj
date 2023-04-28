using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public AudioSource instructions;
    private bool turnSoundOff;

    private void OnTriggerEnter(Collider other)
    {
        if (!turnSoundOff)
        {
            instructions.Play();
            turnSoundOff = true;
        }
    }
}
