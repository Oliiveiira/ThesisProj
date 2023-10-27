using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Transform> placeholders;
    public List<Transform> puzzlePieces;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (AreTransformPositionsEqual())
        {
            Debug.Log("Boa!");
        }
    }

    bool AreTransformPositionsEqual()
    {
        if (placeholders.Count != puzzlePieces.Count)
        {
            return false; // Lists must have the same length to compare positions.
        }

        for (int i = 0; i < placeholders.Count; i++)
        {
            if (placeholders[i].position != puzzlePieces[i].position)
            {
                return false; // If any pair of positions is not equal, return false.
            }
        }

        return true; // All positions are equal.
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (placeholders.Equals(puzzlePieces))
    //    {
    //        Debug.Log("Boa!");
    //    }
    //    //for loop comparing the positions and the pieces of the puzzle, they must have the same index in order to be always right
    //    //for(int i=0; i < placeholders.Count; i++)
    //    //{
    //    //    if(placeholders[i].position == puzzlePieces[i].position)
    //    //    {

    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.Log("Shit");
    //    //    }
    //    //}
    //}
}
