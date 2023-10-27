using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomScanMoney : MonoBehaviour
{
    private bool isInFlag;

    // public Transform coinTransform;

    public TMP_Text totalCost;
    [SerializeField]
    private TMP_Text change;

    public TMP_Text warning;
    // [SerializeField]
    public CustomScanproduct products; //to get the products total

    public FloatSO total;

    [SerializeField]
    private float totalpositive;//to give change

    //Ativate checkChange Objects
    public GameObject ConfirmChange;

    //  public AudioSource coinSound;//trigger the sound


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(total.Value <= 0)
        //{
        //    //totalpositive = total.Value * -1;
        //    //change.SetText("Troco: " + totalpositive.ToString());
        //    //totalCost.SetText("Total: 0");
        //    //total.Value = 0;
        //    //totalCost.SetText("Total: " + total.Value.ToString());
        //}
    }
}
