using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRegisterBox : MonoBehaviour
{
    public GameObject cashRegister;

    public void HideCashRegister()
    {
        StartCoroutine(TimetoHide());
    }

    IEnumerator TimetoHide()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        //After we have waited 5 seconds print the time again.
        cashRegister.SetActive(false);
    }
}
