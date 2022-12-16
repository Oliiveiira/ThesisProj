using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveChange : MonoBehaviour
{
    [SerializeField]
    private FloatSO total;
    [SerializeField]
    private GameObject[] allMoney;
    private GameObject money;
    public Transform changeTransform;

    // Start is called before the first frame update
    void Start()
    {
        allMoney = Resources.LoadAll<GameObject>("Money/");
    }

    // Update is called once per frame
    void Update()
    {
        if(total.Value == -10)
        {
            total.Value += 10;
            money = (GameObject)Instantiate(Resources.Load("Money/10euros"));
            money.transform.position = changeTransform.position;
        }
    }
}
