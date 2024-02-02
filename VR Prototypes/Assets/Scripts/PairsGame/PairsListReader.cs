using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PairsListReader : NetworkBehaviour
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
