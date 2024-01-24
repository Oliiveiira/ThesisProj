using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Oculus.Interaction.HandGrab;

public class PuzzlePieceMultiplayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> rightPosition;
    public bool alreadyPlaced;
    // Start is called before the first frame update
    public NetworkVariable<bool> isInRightPlace = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private AudioSource placeSound;
    [SerializeField]
    private bool hasPlayedSound = false;

    [SerializeField]
    private FloatSO difficultyValue;

    private bool canInteract = false;
    public float delayTime = 3.0f; // Adjust the delay time as needed

    // Ensure that the puzzle piece has an associated NetworkObject
    private NetworkObject networkObject;

    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        StartCoroutine(EnableInteractionAfterDelay());
        //rightPosition = transform.position;
    }

    void Start()
    {
        placeSound = GetComponent<AudioSource>();
        // rightPosition = transform.position;
        //transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(0.3f, 0.7f));
        //transform.rotation = Quaternion.Euler(-90, 0, 0);
        HandGrabInteractable handGrabInteractable = GetComponent<HandGrabInteractable>();
        handGrabInteractable.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (Vector3.Distance(networkObject.transform.position, rightPosition.Value) < difficultyValue.Value/*0.035f*/)
            {
                isInRightPlace.Value = true;
                //transform.position = rightPosition;
                networkObject.transform.position = rightPosition.Value;
                networkObject.transform.rotation = Quaternion.Euler(0, -90, 90);

                if (!hasPlayedSound)
                {
                    placeSound.Play();
                    hasPlayedSound = true;  // Set the flag to indicate that the sound has been played
                }
            }
            else
            {
                hasPlayedSound = false;
                isInRightPlace.Value = false;
            }
            //if (isInRightPlace)
            //{
            //    placeSound.Play();
            //}
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetClientOwnershipServerRPC();
        }
    }

    private IEnumerator EnableInteractionAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        // Allow interaction after the delay
        canInteract = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PuzzlePlace"))
        {
            if (networkObject.transform.position == other.transform.position && canInteract)
            {
                Debug.Log("OnTriggerStay Active");
                //placeSound.Play();
                //HandGrabInteractable handGrabInteractable = GetComponent<HandGrabInteractable>();
                //handGrabInteractable.enabled = false;
                //MeshRenderer puzzlePlaceRenderer = other.GetComponent<MeshRenderer>();
                //puzzlePlaceRenderer.enabled = false;
                DisableInteractableClientRpc(other.gameObject.name);
            }
        }
    }

    [ClientRpc]
    private void DisableInteractableClientRpc(string objectName)
    {
        // Find the GameObject using the identifier
        GameObject other = GameObject.Find(objectName);
        placeSound.Play();
        if (other != null)
        {
            HandGrabInteractable handGrabInteractable = GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }

            MeshRenderer puzzlePlaceRenderer = other.GetComponent<MeshRenderer>();
            if (puzzlePlaceRenderer != null)
            {
                puzzlePlaceRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInRightPlace.Value)
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
