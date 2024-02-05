using Unity.Netcode;
using UnityEngine;

public class TherapistPositionChanger : MonoBehaviour
{
    public void ChangeCameraPosition(int index) 
    {
        IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.ChangeCameraPosition(index);
    }
}
