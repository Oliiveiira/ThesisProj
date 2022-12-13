using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Products/New Product")] 
public class ObjectSO : ScriptableObject
{
    //ScriptableObject to keep float data from objects(ex:Hp)
    [SerializeField]
    public string productName;
    public float productCost;
}
