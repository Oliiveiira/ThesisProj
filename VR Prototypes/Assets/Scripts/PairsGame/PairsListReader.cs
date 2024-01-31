using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairsListReader : MonoBehaviour
{
    [System.Serializable]
    public class Pairs
    {
        public string pairName;
        public string pairPath;
        public List<string> pieces;
    }

    [System.Serializable]
    public class PairsList
    {
        public List<Pairs> pairlevel;
    }

    public PairsList myPairsList = new PairsList();
}
