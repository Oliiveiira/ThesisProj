using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] list;


    public void HideList(bool isHidden)
    {
        if (isHidden)
        {
            for(int i = 0; i < list.Length; i++)
            {
                list[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i].SetActive(true);
            }
        }
    }
}
