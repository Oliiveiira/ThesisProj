using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPuzzleNumberOfCubes : MonoBehaviour
{
    [SerializeField]
    private PuzzleListWritter puzzleList;
    [SerializeField]
    private IntSO puzzleLevelSO;

    public void Set4CubesScene()
    {
        puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes = 4;
        puzzleList.SavePuzzleListToJson();
    }
    public void Set9CubesScene()
    {
        puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes = 9;
        puzzleList.SavePuzzleListToJson();
    }
    public void Set16CubesScene()
    {
        puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].numberOfCubes = 16;
        puzzleList.SavePuzzleListToJson();
    }
}
