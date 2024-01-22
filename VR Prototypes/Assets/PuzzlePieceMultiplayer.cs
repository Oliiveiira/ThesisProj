using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Oculus.Interaction.HandGrab;

public class PuzzlePieceMultiplayer : NetworkBehaviour
{
    public Vector3 rightPosition;
    public bool alreadyPlaced;
    // Start is called before the first frame update
    public bool isInRightPlace;
    private AudioSource placeSound;
    [SerializeField]
    private bool hasPlayedSound = false;

    [SerializeField]
    private FloatSO difficultyValue;

    private void Awake()
    {
        //rightPosition = transform.position;
    }

    void Start()
    {
        placeSound = GetComponent<AudioSource>();
        // rightPosition = transform.position;
        //transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(0.3f, 0.7f));
        //transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, rightPosition) < difficultyValue.Value /*0.035f*/)
        {
            isInRightPlace = true;
            transform.position = rightPosition;
            transform.rotation = Quaternion.Euler(0, -90, 90);

            if (!hasPlayedSound)
            {
                placeSound.Play();
                hasPlayedSound = true;  // Set the flag to indicate that the sound has been played
            }
        }
        else
        {
            hasPlayedSound = false;
            isInRightPlace = false;
        }
        //if (isInRightPlace)
        //{
        //    placeSound.Play();
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetClientOwnershipServerRPC();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PuzzlePlace"))
        {
            if (transform.position == other.transform.position)
            {
                //placeSound.Play();
                HandGrabInteractable handGrabInteractable = GetComponent<HandGrabInteractable>();
                handGrabInteractable.enabled = false;
                MeshRenderer puzzlePlaceRenderer = other.GetComponent<MeshRenderer>();
                puzzlePlaceRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInRightPlace)
        {
            MeshRenderer puzzlePlace = other.GetComponent<MeshRenderer>();
            puzzlePlace.enabled = true;
        }
    }

    public bool ChangeOwnership()
    {
        // If it already is the owner or another player is the owner don't do anything
        if (IsOwner || !IsOwnedByServer) return false;

        SetClientOwnershipServerRPC();
        // Send a request to the server to change ownership
        //ChangeOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
        return true;
    }

    public bool RemoveOwnership(float delay = 0.08f)
    {
        // If it isn't the owner don't do anything
        if (!IsOwner) return false;

        // Use a server RPC to remove ownership after a delay
        StartCoroutine(DelayRemoveOwnership(delay));
        return true;
    }

    IEnumerator DelayRemoveOwnership(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Send a request to the server to remove ownership
        RemoveOwnershipServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnershipServerRpc(ulong localClientId)
    {
        // Check ownership on the server and change it
        if (IsOwner)
        {
            GetComponent<NetworkObject>().ChangeOwnership(localClientId);
            Debug.Log("Changed ownership");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveOwnershipServerRpc()
    {
        // Check ownership on the server and remove it
        if (IsOwner)
        {
            GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            Debug.Log("Removed ownership");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
            Debug.Log("Client is now the owner");
        }
        else
        {
            Debug.Log("not working");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveOwnershipServerRPC(ServerRpcParams serverRpcParams = default)
    {
        var networkObject = GetComponent<NetworkObject>();

        // Check if the object has an owner
        if (networkObject.IsOwner)
        {
            // Remove ownership from the current owner
            networkObject.RemoveOwnership();
            Debug.Log("Ownership removed");
        }
        else
        {
            Debug.Log("Object doesn't have an owner");
        }
    }

}
