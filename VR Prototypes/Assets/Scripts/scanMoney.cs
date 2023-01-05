using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scanMoney : MonoBehaviour
{
    private bool isInFlag;

   // public Transform coinTransform;

    public TMP_Text totalCost;
    [SerializeField]
    private TMP_Text change;
    [SerializeField]
    private ScanProduct products; //to get the products total

    public FloatSO total;

    [SerializeField]
    private float totalpositive;//to give change

    //  public AudioSource coinSound;//trigger the sound

    public GrabInteractor grabInteractor;//to force release
    public HandGrabInteractor handgrabInteractorR;//to force release
    public HandGrabInteractor handgrabInteractorL;//to force release


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(total.Value <= 0)
        {
            totalpositive = total.Value * -1;
            change.SetText("Troco: " + totalpositive.ToString());
            totalCost.SetText("Total: 0");
            //total.Value = 0;
            //totalCost.SetText("Total: " + total.Value.ToString());
        }
    }
}
