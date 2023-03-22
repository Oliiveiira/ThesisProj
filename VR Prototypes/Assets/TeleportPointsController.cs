using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointsController : MonoBehaviour
{
    public Transform[] tpPoints;

    public void DeactivateTeleportPoint()
    {
        for(int i = 0; i< tpPoints.Length; i++)
        {
            tpPoints[i].gameObject.SetActive(false);
        }
    }

    public void ActivateTeleportPoint()
    {
        for (int i = 0; i < tpPoints.Length; i++)
        {
            tpPoints[i].gameObject.SetActive(true);
        }
    }
}
