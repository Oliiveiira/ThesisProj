using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Marker : NetworkBehaviour
{
    private Renderer render;
    public Vector3 initialTransform;
    private TextureSender textureSender;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        initialTransform = transform.localPosition;
        textureSender = FindObjectOfType<TextureSender>();
        Debug.Log(initialTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pallete"))
        {
            Debug.Log("PickColor");
            Renderer palleteRenderer = other.GetComponent<Renderer>();
            render.material.color = palleteRenderer.material.color;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.localPosition = new Vector3(0,0,0);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            transform.parent.GetComponent<NetworkObject>().ChangeOwnership(clientId);
            Debug.Log("Client is now the owner");
            Debug.Log(clientId);
        }
        else
        {
            Debug.Log("not working");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientHandMarkerOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
            Debug.Log("Client is now the owner");
            Debug.Log(clientId);
        }
        else
        {
            Debug.Log("not working");
        }
    }

    public void SetOwner()
    {
        SetClientHandMarkerOwnershipServerRPC();
        transform.localPosition = initialTransform;
    }

    public void SetHandMarkerPosition()
    {
        transform.localPosition = initialTransform;
    }

    private void OnTriggerStay(Collider other)
    {
       // if(IsOwner)
       // {
            if (other.gameObject.CompareTag("Canvas"))
            {
                textureSender.StartSending();
                //transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);

                Debug.Log("Collided");
            }
       // }
    }

    private void OnTriggerExit(Collider other)
    {
       // if (IsOwner)
       // {
            if (other.gameObject.CompareTag("Canvas"))
            {
                textureSender.StopSending();
                //transform.localPosition = initialTransform;
                Debug.Log("ExitCollision");
            }
       // }
    }
}
