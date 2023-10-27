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
        set { _value = RoundToThreeDecimalPlaces(value); }
    }

    // Custom rounding method
    private float RoundToThreeDecimalPlaces(float floatValue)
    {
        // Use Mathf.Round to round to three decimal places
        return Mathf.Round(floatValue * 1000f) / 1000f;
    }
}
