using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New SO/IntSO")]
public class IntSO : ScriptableObject
{
    //ScriptableObject to keep float data from objects(ex:Hp)
    [SerializeField]
    private int _value;

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }

}