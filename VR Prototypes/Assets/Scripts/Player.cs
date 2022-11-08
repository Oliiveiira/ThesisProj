using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 initialPos = new(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayerPos()
    {
        transform.position = initialPos;
    }
}
