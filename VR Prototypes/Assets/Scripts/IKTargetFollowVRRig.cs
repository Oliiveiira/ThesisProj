using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Oculus.Interaction.Input;
using System.Collections.Generic;
using static PuzzleDeskPivotReader;
using System.IO;
using TMPro;
using System.Collections;

//[System.Serializable]
//public class VRMap: NetworkBehaviour
//{
//    public Transform vrTarget;
//    public Transform ikTarget;
//    public Vector3 trackingPositionOffset;
//    public Vector3 trackingRotationOffset;

//    public void Map()
//    {
//        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
//        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
//    }
//}

public class IKTargetFollowVRRig : NetworkBehaviour
{
    //First define some global variables in order to speed up the Update() function
    GameObject myXRRig;
    Transform leftVRTarget, rightVRTarget, headVRTarget, centerEyeAnchor, leftHandAnchor, rightHandAnchor;
    // public Transform leftVRTarget;
    public Transform leftIKTarget;
    //public Transform rightVRTarget;
    public Transform rightIKTarget;
    //public Transform headVRTarget;
    public Transform headIKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public SkinnedMeshRenderer avatarRenderer;
    public TextMeshPro messagePlaceholder;

    [Range(0, 1)]
    public float turnSmoothness = 0.1f;
    //public VRMap head;
    //public VRMap leftHand;
    //public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    public NetworkVariable<Platform> playerPlatform = new NetworkVariable<Platform>(Platform.Patient, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public GameObject therapistSpawnLocs = null;
    public Camera therapistCamera = null;
    public int currentIndex = 0;
    public GameObject headFollowing = null;
    public GameObject arrow = null;
    public GameObject arrowPrefab = null;
    private float deskHeight = 0;

    private NetworkObject grabbedNetworkObject = null;
    private ulong originalOwner = 0;
    private Vector3 initialMovePosition = Vector3.zero;
    
    private float currentTime = 0;
    public int currentPaintPosition;
    public int spawnLocationsNumber;

    public GameObject therapistMarkerPrefab = null;
    public GameObject therapistMarker = null;

    public void Awake()
    {
        arrowPrefab = Resources.Load<GameObject>("Arrow");
        therapistMarkerPrefab = Resources.Load<GameObject>("TherapistMarker");
        string jsonFileName = "PuzzleDeskPivot.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // Check if the file exists in the persistent data path
        string jsonText = File.ReadAllText(jsonFilePath);
        DeskPivot myDeskPivot = JsonUtility.FromJson<DeskPivot>(jsonText);
        deskHeight = myDeskPivot.deskPivotY;
    }

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        NetworkManager.SceneManager.OnLoadComplete += OnChangeScene;

        var myID = transform.GetComponent<NetworkObject>().NetworkObjectId;
        if (IsOwnedByServer)
            transform.name = "Host:" + myID;    //this must be the host
        else
            transform.name = "Client:" + myID; //this must be the client 

        if (!IsOwner) return;

        playerPlatform.Value = PlatformPicker.localPlatform;

        if (PlatformPicker.localPlatform == Platform.Therapist) {
            therapistSpawnLocs = GameObject.Find("TherapistSpawnLocs");
            therapistCamera = GameObject.Find("TherapistCamera").GetComponent<Camera>();
            SetRendererTherapistServerRpc();
            return;
        }

        myXRRig = GameObject.Find("InputOVR");
        if (myXRRig) Debug.Log("Found InputOVR");
        else Debug.Log("Could not find OVRCameraRig!");

        transform.rotation = Quaternion.Euler(-90, 0, 0);

        leftHandAnchor = GameObject.Find("LeftHandAnchor").transform;
        Debug.Log(leftHandAnchor);
        rightHandAnchor = GameObject.Find("RightHandAnchor").transform;
        Debug.Log(rightHandAnchor);
        centerEyeAnchor = GameObject.Find("CenterEyeAnchor").transform;
        Debug.Log(centerEyeAnchor);

        leftVRTarget = leftHandAnchor.transform.GetChild(2);
        Debug.Log(leftVRTarget);
        rightVRTarget = rightHandAnchor.transform.GetChild(2);
        Debug.Log(rightVRTarget);
        headVRTarget = centerEyeAnchor.transform.GetChild(2);
        Debug.Log(headVRTarget);

        GameObject spawnLocations = GameObject.Find("SpawnLocations");
        //Turn on if painting levels dont work
        //myXRRig.transform.parent.position = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).position;
        //myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).rotation;
        spawnLocationsNumber = spawnLocations.transform.childCount;

        if (SceneManager.GetActiveScene().name != "PaintMultiplayerHub")
        {
            myXRRig.transform.parent.position = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).position;
            myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).rotation;
        }
        else
        {
            myXRRig.transform.parent.position = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).position;
            myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).rotation;
        }
        Debug.Log($"Spawned to location {spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).position} and rotation {spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).rotation}");

    }

    private void Start()
    {
        if (IsLocalPlayer || playerPlatform.Value == Platform.Therapist)
        {
            // If this is the local player, hide the avatar for their own vision
            avatarRenderer.enabled = false;
        }
        else
        {
            avatarRenderer.enabled = true;
            // If this is a remote player, request the server to show their avatar
          //  ShowAvatarServerRpc();
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (playerPlatform.Value == Platform.Therapist)
        {
            // Debug.Log("entering here");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeCameraPosition(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeCameraPosition(1);
            }
            if (headFollowing != null) 
            {
                therapistCamera.transform.position = headFollowing.transform.position;
                therapistCamera.transform.rotation = headFollowing.transform.rotation;
            }

            //For PaintingLevels
            if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && currentIndex == 1 && Input.mousePosition.y >= 175.1f && SceneManager.GetActiveScene().name == "PaintingLevels2")
            {
                Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.725f));
                //Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(Input.GetTouch(0));
                placeToMove.y = deskHeight;
                SpawnTherapistMarkerServerRpc(placeToMove, Quaternion.Euler(90,0,0));
                
                RaycastHit hit;
                Ray ray = therapistCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Debug.Log(hit.collider.name);
                    grabbedNetworkObject = hit.collider.gameObject.GetComponent<NetworkObject>();
                    originalOwner = grabbedNetworkObject.OwnerClientId;
                    SetObjectOwnershipTherapistServerRpc(new NetworkObjectReference(grabbedNetworkObject), NetworkManager.LocalClientId, true);
                    initialMovePosition = grabbedNetworkObject.transform.position;
                }
            }

            if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && currentIndex == 1 && Input.mousePosition.y >= 175.1f && SceneManager.GetActiveScene().name == "PaintingLevels2")
            {
                therapistMarker.transform.position = therapistCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.725f));
            }

            if (Input.GetMouseButtonUp(0) && therapistMarker != null && currentIndex == 1 && Input.mousePosition.x >= 615 && SceneManager.GetActiveScene().name == "PaintingLevels2")
            {
                ClearTherapistMarkerServerRpc();
            }

            if (Input.GetMouseButton(0) && grabbedNetworkObject != null && grabbedNetworkObject.IsOwner&& SceneManager.GetActiveScene().name != "CustomSupermarketMultiplayerLevel")
            {
                grabbedNetworkObject.transform.position = therapistCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, therapistCamera.transform.position.y - grabbedNetworkObject.transform.position.y));
            }

            if (Input.GetMouseButtonUp(0) && grabbedNetworkObject != null && grabbedNetworkObject.IsOwner && SceneManager.GetActiveScene().name != "CustomSupermarketMultiplayerLevel")
            {
                grabbedNetworkObject.transform.position = initialMovePosition;
                StartCoroutine(DelayRemoveOwnership(1, grabbedNetworkObject));
                grabbedNetworkObject = null;
            }

            if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && currentIndex == 1 && Input.mousePosition.y >= 175.1f && SceneManager.GetActiveScene().name != "CustomSupermarketMultiplayerLevel" && SceneManager.GetActiveScene().name != "PaintingLevels2") 
            {
                Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, therapistCamera.nearClipPlane + 0.2f * 2));
                //Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(Input.GetTouch(0));
                placeToMove.y = deskHeight + 0.2f;
                SpawnArrowServerRpc(placeToMove, Quaternion.Euler(0,0,90));

                RaycastHit hit;
                Ray ray = therapistCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100f)) 
                {
                    Debug.Log(hit.collider.name);
                    grabbedNetworkObject = hit.collider.gameObject.GetComponent<NetworkObject>();
                    originalOwner = grabbedNetworkObject.OwnerClientId;
                    SetObjectOwnershipTherapistServerRpc(new NetworkObjectReference(grabbedNetworkObject), NetworkManager.LocalClientId, true);
                    initialMovePosition = grabbedNetworkObject.transform.position;
                }
            }
            else if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && currentIndex == 1 && Input.mousePosition.y >= 175.1f && SceneManager.GetActiveScene().name == "CustomSupermarketMultiplayerLevel")
            {
                Debug.Log("spawn arrow");
                Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, therapistCamera.nearClipPlane + 1.9f));
                placeToMove.x = 8.5f;
                SpawnArrowServerRpc(placeToMove, Quaternion.Euler(0, 180, 0));
            }
            return;
        }
    }

    [ServerRpc]
    public void SetObjectOwnershipTherapistServerRpc(NetworkObjectReference objectReference, ulong clientId, bool isTherapist)
    {
        NetworkObject networkObject;
        if (objectReference.TryGet(out networkObject))
        {
            networkObject.ChangeOwnership(clientId);
            CheckBoxCollidersClientRpc(objectReference, isTherapist);
        }
    }

    [ClientRpc]
    public void CheckBoxCollidersClientRpc(NetworkObjectReference objectReference, bool isTherapist) 
    {
        NetworkObject networkObject;
        if (objectReference.TryGet(out networkObject))
        {
            networkObject.GetComponent<BoxCollider>().enabled = !isTherapist;
        }
    }

    [ServerRpc]
    void ShowAvatarServerRpc(ServerRpcParams rpcParams = default)
    {
        // Show the avatar on the server
        avatarRenderer.enabled = true;
    }

    [ServerRpc]
    void SpawnTherapistMarkerServerRpc(Vector3 placeToMove, Quaternion rotation)
    {
        if (therapistMarker == null)
        {
            therapistMarker = Instantiate(therapistMarkerPrefab, placeToMove, rotation/*Quaternion.Euler(0,0,90)*/);
            therapistMarker.GetComponent<NetworkObject>().Spawn();
            return;
        }
       // therapistMarker.transform.position = placeToMove;
    }

    [ServerRpc]
    public void ClearTherapistMarkerServerRpc()
    {
        if (therapistMarker == null)
            return;

        Destroy(therapistMarker);
    }

    [ServerRpc]
    void SpawnArrowServerRpc(Vector3 placeToMove, Quaternion rotation)
    {
        if (arrow == null) 
        {
            arrow = Instantiate(arrowPrefab, placeToMove, rotation/*Quaternion.Euler(0,0,90)*/);
            arrow.GetComponent<NetworkObject>().Spawn();
            return;
        }
        arrow.transform.position = placeToMove;
    }

    [ServerRpc]
    public void ClearArrowServerRpc()
    {
        if (arrow == null)
            return;

        Destroy(arrow);
    }

    public void Map()
    {
        //ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        //ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        leftIKTarget.position = leftVRTarget.TransformPoint(trackingPositionOffset);
        leftIKTarget.rotation = leftVRTarget.rotation * Quaternion.Euler(trackingRotationOffset);

        rightIKTarget.position = rightVRTarget.TransformPoint(trackingPositionOffset);
        rightIKTarget.rotation = rightVRTarget.rotation * Quaternion.Euler(trackingRotationOffset);

        headIKTarget.position = headVRTarget.TransformPoint(trackingPositionOffset);
        headIKTarget.rotation = headVRTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsOwner) return;
        if (playerPlatform.Value == Platform.Therapist) return;
        if (!myXRRig) return;

        //transform.position = head.ikTarget.position + headBodyPositionOffset;
        transform.position = headIKTarget.position + headBodyPositionOffset;

        float yaw = headVRTarget.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);

        Map();

        //head.Map();
        //leftHand.Map();
        //rightHand.Map();
    }

    public void OnChangeScene(ulong clientId, string sceneName = "", LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        currentTime = 0;
        Debug.Log($"{clientId} - ChangeScene");
        if (!IsOwner) return;
        if (PlatformPicker.localPlatform == Platform.Therapist)
        {
            therapistSpawnLocs = GameObject.Find("TherapistSpawnLocs");
            therapistCamera = GameObject.Find("TherapistCamera").GetComponent<Camera>();
            return;
        }

        myXRRig = GameObject.Find("InputOVR");
        if (myXRRig) Debug.Log("Found InputOVR");
        else Debug.Log("Could not find OVRCameraRig!");
        transform.rotation = Quaternion.Euler(-90, 0, 0);

        leftHandAnchor = GameObject.Find("LeftHandAnchor").transform;
        Debug.Log(leftHandAnchor);
        rightHandAnchor = GameObject.Find("RightHandAnchor").transform;
        Debug.Log(rightHandAnchor);
        centerEyeAnchor = GameObject.Find("CenterEyeAnchor").transform;
        Debug.Log(centerEyeAnchor);

        leftVRTarget = leftHandAnchor.transform.GetChild(2);
        Debug.Log(leftVRTarget);
        rightVRTarget = rightHandAnchor.transform.GetChild(2);
        Debug.Log(rightVRTarget);
        headVRTarget = centerEyeAnchor.transform.GetChild(2);
        Debug.Log(headVRTarget);

        GameObject spawnLocations = GameObject.Find("SpawnLocations");
        //Turn on if painting levels dont work
        //myXRRig.transform.parent.position = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).position;
        //myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).rotation;
        if (SceneManager.GetActiveScene().name != "PaintingLevels2")
        {
            myXRRig.transform.parent.position = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).position;
            myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + 1) % spawnLocations.transform.childCount).rotation;
        }
        else
        {
            myXRRig.transform.parent.position = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + currentPaintPosition) % spawnLocations.transform.childCount).position;
            myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(((int)NetworkManager.LocalClientId + currentPaintPosition) % spawnLocations.transform.childCount).rotation;
            if(myXRRig.transform.parent.position == spawnLocations.transform.GetChild(0).position)
            {
                GetComponent<TextureSender>().enabled = true;
            }
        }
        Debug.Log($"Spawned to location {spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).position} and rotation {spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).rotation}");
    }

    public void ChangeCameraPosition(int index)
    {
        currentIndex = index;
        if (index <= 1)
        {
            therapistCamera.transform.position = therapistSpawnLocs.transform.GetChild(index).position;
            therapistCamera.transform.rotation = therapistSpawnLocs.transform.GetChild(index).rotation;
            headFollowing = null;
            return;
        }
        GetPlayerHeadTargetServerRpc(index - 2, NetworkManager.Singleton.LocalClientId);
    }

    public void StartTimer(TextMeshProUGUI timer)
    {
        currentTime += 1 * Time.deltaTime;
        timer.SetText(currentTime.ToString("0"));
    }

    [ServerRpc]
    public void SendMessageToPlayerServerRpc(int targetId, string message)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { (ulong)targetId }
            }
        };

        IKTargetFollowVRRig playerTarget = (NetworkManager.ConnectedClientsList[targetId].PlayerObject.GetComponent<IKTargetFollowVRRig>());
        playerTarget.SendMessageToPlayerClientRpc(message, clientRpcParams);
    }

    [ClientRpc]
    public void SendMessageToPlayerClientRpc(string message, ClientRpcParams clientRpcParams = default)
    {
        // TODO: fazer alguma coisa com a mensagem
        messagePlaceholder.gameObject.SetActive(true);
        messagePlaceholder.text = message;
        Debug.Log(message);
    }

    [ServerRpc]
    public void SetRendererTherapistServerRpc() 
    {
        SetRendererTherapistClientRpc();
    }

    [ClientRpc]
    public void SetRendererTherapistClientRpc() 
    {
        avatarRenderer.enabled = false;
    }

    [ServerRpc]
    public void GetPlayerHeadTargetServerRpc(int targetId, ulong clientId)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { (ulong)clientId }
            }
        };

        NetworkBehaviourReference playerTarget = new NetworkBehaviourReference(NetworkManager.ConnectedClientsList[targetId + 1].PlayerObject.GetComponent<NetworkBehaviour>());
        SetPlayerHeadTargetClientRpc(playerTarget, clientRpcParams);
    }

    [ClientRpc]
    public void SetPlayerHeadTargetClientRpc(NetworkBehaviourReference playerTarget, ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner) return;
        NetworkBehaviour playerNetwork;
        if (playerTarget.TryGet<NetworkBehaviour>(out playerNetwork)) 
        {
            // Note: If player is changes place this breaks
            headFollowing = playerNetwork.gameObject.transform.GetChild(1).GetChild(2).GetChild(0).gameObject;
        }
        Debug.Log("Failed to get Network Player");
    }

    IEnumerator DelayRemoveOwnership(float delay, NetworkObject puzzlePiece)
    {
        yield return new WaitForSeconds(delay);

        // Send a request to the server to remove ownership
        SetObjectOwnershipTherapistServerRpc(new NetworkObjectReference(puzzlePiece), originalOwner, false);
    }
}
