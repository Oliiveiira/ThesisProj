using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TherapistPositionChanger : MonoBehaviour
{
    public TMP_InputField textInput;
    public GameObject textInputObject;

    public void ChangeCameraPosition(int index) 
    {
        if (index >= 2) 
        {
            textInputObject.SetActive(true);
        }
        else
        {
            textInputObject.SetActive(false);
        }

        IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.ChangeCameraPosition(index);
    }

    public void ClearArrow()
    {
        IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.ClearArrowServerRpc();
    }

    public void SendText() 
    {
        IKTargetFollowVRRig player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
        player.SendMessageToPlayerServerRpc(player.currentIndex - 2, textInput.text);
    }
}
