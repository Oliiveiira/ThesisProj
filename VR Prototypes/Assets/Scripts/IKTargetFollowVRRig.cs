using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Oculus.Interaction.Input;
using System.Collections.Generic;
using static PuzzleDeskPivotReader;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using TMPro;

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

    public void Awake()
    {
        arrowPrefab = Resources.Load<GameObject>("Arrow");
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
        myXRRig.transform.parent.position = spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).position;
        myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).rotation;
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
            if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && currentIndex == 1 && Input.mousePosition.y >= 175.1f) 
            {
                Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, therapistCamera.nearClipPlane + 0.2f * 2));
                //Vector3 placeToMove = therapistCamera.ScreenToWorldPoint(Input.GetTouch(0));
                placeToMove.y = deskHeight + 0.2f;
                Debug.Log(Input.mousePosition);
                SpawnArrowServerRpc(placeToMove);
            }
            return;
        }
    }

    [ServerRpc]
    void ShowAvatarServerRpc(ServerRpcParams rpcParams = default)
    {
        // Show the avatar on the server
        avatarRenderer.enabled = true;
    }

    [ServerRpc]
    void SpawnArrowServerRpc(Vector3 placeToMove)
    {
        if (arrow == null) 
        {
            arrow = Instantiate(arrowPrefab, placeToMove, Quaternion.Euler(0,0,90));
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
        myXRRig.transform.parent.position = spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).position;
        myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild((int)NetworkManager.LocalClientId % spawnLocations.transform.childCount).rotation;
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

        NetworkBehaviourReference playerTarget = new NetworkBehaviourReference(NetworkManager.ConnectedClientsList[targetId].PlayerObject.GetComponent<NetworkBehaviour>());
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
}
