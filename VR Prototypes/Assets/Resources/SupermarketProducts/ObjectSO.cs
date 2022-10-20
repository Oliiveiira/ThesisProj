using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] 
public class ObjectSO : ScriptableObject
{
    //ScriptableObject to keep float data from objects(ex:Hp)
    [SerializeField]
    public string objectName;

    //public string Name
    //{
    //    get { return _name; }
    //    set { _name = value; }
    //}

}
