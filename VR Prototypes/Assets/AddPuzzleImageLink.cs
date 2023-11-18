using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddPuzzleImageLink : PuzzleListReader
{
    public TextMeshProUGUI link;

    [SerializeField]
    private PuzzleListWritter puzzleList;
    [SerializeField]
    private IntSO puzzleLevelSO;

    public void AddPuzzleImageURL()
    {
        puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].textureURL = link.text;
        puzzleList.SavePuzzleListToJson();
    }
}
