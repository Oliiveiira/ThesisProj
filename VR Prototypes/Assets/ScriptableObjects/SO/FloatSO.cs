using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New SO/FloatSO")]
public class FloatSO : ScriptableObject
{
    //ScriptableObject to keep float data from objects(ex:Hp)
    [SerializeField]
    private float _value;

    public float Value
    {
        get { return _value; }
        set { _value = value; }
    }

}
