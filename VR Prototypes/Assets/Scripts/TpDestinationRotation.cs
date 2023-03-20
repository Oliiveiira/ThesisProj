using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpDestinationRotation : MonoBehaviour
{
    public Transform player;
    private MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.enabled = false;
            //  player.rotation = Quaternion.Euler(0, 90, 0);
            player.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0, 90, 0), 2f);
        }
    }

    public void ActivateTPPoint()
    {
        mesh.enabled = true;
    }
}
