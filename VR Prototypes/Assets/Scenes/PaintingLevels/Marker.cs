using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Marker : NetworkBehaviour
{
    private Renderer render;
    public Vector3 initialTransform;
    private TextureSender textureSender;

    public float syncInterval = 0.05f; // Sync every 0.05 seconds (20 times per second)
    private float lastSyncTime;

    public NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool isSending = false;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        initialTransform = transform.localPosition;
        textureSender = FindObjectOfType<TextureSender>();
        Debug.Log(initialTransform);
    }

    private void Update()
    {
        if (IsOwner)
        {
            networkedPosition.Value = transform.position;
        }
        else if (!IsOwner)
        {
            transform.position = networkedPosition.Value;
        }
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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        transform.localPosition = new Vector3(0,0,0);
    //    }
    //}

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
        textureSender.OwnerclientID = clientId;
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
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Canvas"))
            {
                textureSender.StartSending();
                Debug.Log("Collided");
            }
        }

        if (other.gameObject.CompareTag("Canvas"))
        {
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Canvas"))
            {
                textureSender.StopSending();
                Debug.Log("Collided");
            }
        }

        if (other.gameObject.CompareTag("Canvas"))
        {
            transform.localPosition = initialTransform;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (IsOwner)
    //    {
    //        if (other.gameObject.CompareTag("Canvas"))
    //        {
    //            isSending = true;
    //            Debug.Log("Collided");
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (IsOwner)
    //    {
    //        if (other.gameObject.CompareTag("Canvas"))
    //        {
    //            isSending = false;
    //            Debug.Log("Collided");
    //        }
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //   // if(IsOwner)
    //   // {
    //        if (other.gameObject.CompareTag("Canvas"))
    //        {
    //            textureSender.StartSending();
    //            //transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);

    //            Debug.Log("Collided");
    //        }
    //   // }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //   // if (IsOwner)
    //   // {
    //        if (other.gameObject.CompareTag("Canvas"))
    //        {
    //            textureSender.StopSending();
    //            //transform.localPosition = initialTransform;
    //            Debug.Log("ExitCollision");
    //        }
    //   // }
    //}
}
