using UnityEngine;

public class PlatformPicker : MonoBehaviour
{
    public static Platform localPlatform = Platform.Patient;
    public Platform choosenPlatform = Platform.Patient;

    private void Awake()
    {
        localPlatform = choosenPlatform;
    }
}
