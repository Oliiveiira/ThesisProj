using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField]
    private FloatSO money; //ScriptableObject with the product data
    public float value;

    // Start is called before the first frame update
    private void Awake()
    {
        value = money.Value;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(objects.objectName);
    }
}
