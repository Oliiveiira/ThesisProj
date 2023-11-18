using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleListReader : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleLevel
    {
        public string textureURL;
        public int numberOfCubes;
    }

    [System.Serializable]
    public class PuzzleList
    {
        public List<PuzzleLevel> puzzlelevel;
    }

    public PuzzleList myPuzzleList = new PuzzleList();
}
