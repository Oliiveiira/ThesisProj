using System;
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
        string inputUrl = link.text;

        // Extract the file ID from the input URL
        string fileId = ExtractFileIdFromUrl(inputUrl);

        // Create the Google Drive direct download link
        string downloadLink = $"https://drive.google.com/uc?export=download&id={fileId}";

        // Save the download link to the puzzleList
        puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].textureURL = downloadLink;

       // puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].textureURL = link.text;
        puzzleList.SavePuzzleListToJson();
    }

    private string ExtractFileIdFromUrl(string url)
    {
        // Extract the file ID from the URL using string.Split
        string[] parts = url.Split(new char[] { '/', '?' }, StringSplitOptions.RemoveEmptyEntries);
        int index = Array.IndexOf(parts, "d");
        if (index != -1 && index + 1 < parts.Length)
        {
            return parts[index + 1];
        }
        return null;
    }

    //public TextMeshProUGUI link;

    //[SerializeField]
    //private PuzzleListWritter puzzleList;
    //[SerializeField]
    //private IntSO puzzleLevelSO;

    //public void AddPuzzleImageURL()
    //{
    //    puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].textureURL = link.text;
    //    puzzleList.SavePuzzleListToJson();
    //}
}
