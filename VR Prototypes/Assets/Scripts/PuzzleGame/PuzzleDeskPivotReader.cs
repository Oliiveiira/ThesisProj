using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDeskPivotReader : MonoBehaviour
{
    [System.Serializable]
    public class DeskPivot
    {
        public float deskPivotX;
        public float deskPivotY;
    }

    public DeskPivot myDeskPivot = new DeskPivot();
}
