using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlatformObjects : MonoBehaviour
{
    
    public List<GameObject> patientObjects;
    public List<GameObject> therapistObjects;

    void Start()
    {
        if (PlatformPicker.localPlatform == Platform.Patient) 
        {
            DisableEnableObjects(therapistObjects, patientObjects);           
        }
        else if (PlatformPicker.localPlatform == Platform.Therapist)
        {
            DisableEnableObjects(patientObjects, therapistObjects);
        }
    }

    void DisableEnableObjects(List<GameObject> disableObjects, List<GameObject> enableObjects) 
    {
        foreach(GameObject disableObject in disableObjects) 
        {
            disableObject.SetActive(false);
        }

        foreach (GameObject enableObject in enableObjects)
        {
            enableObject.SetActive(true);
        }
    }

}
