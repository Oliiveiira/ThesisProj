using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Oculus.Interaction.Input;

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

    [Range(0, 1)]
    public float turnSmoothness = 0.1f;
    //public VRMap head;
    //public VRMap leftHand;
    //public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

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
        int i = 0;
        foreach (NetworkClient player in NetworkManager.ConnectedClients.Values) 
        {
            if (player.ClientId == NetworkManager.LocalClientId) 
            {
                myXRRig.transform.parent.position = spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).position;
                myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).rotation;
                Debug.Log($"Spawned to location {spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).position} and rotation {spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).rotation}");
                break;
            }
            i++;
        }
    }

    private void Start()
    {
        if (IsLocalPlayer)
        {
            // If this is the local player, hide the avatar for their own vision
            avatarRenderer.enabled = false;
        }
        else
        {
            // If this is a remote player, request the server to show their avatar
            ShowAvatarServerRpc();
        }
    }

    [ServerRpc]
    void ShowAvatarServerRpc(ServerRpcParams rpcParams = default)
    {
        // Show the avatar on the server
        avatarRenderer.enabled = true;
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
        int i = 0;
        foreach (NetworkClient player in NetworkManager.ConnectedClients.Values)
        {
            if (player.ClientId == NetworkManager.LocalClientId)
            {
                myXRRig.transform.parent.position = spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).position;
                myXRRig.transform.parent.rotation = spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).rotation;
                Debug.Log($"Spawned to location {spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).position} and rotation {spawnLocations.transform.GetChild(i % spawnLocations.transform.childCount).rotation}");
                break;
            }
            i++;
        }
    }
}
