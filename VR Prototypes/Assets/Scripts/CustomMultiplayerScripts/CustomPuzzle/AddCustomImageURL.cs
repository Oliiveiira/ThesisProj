using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AddCustomImageURL : LevelsDataWritter
{
    public TextMeshProUGUI link;

    private void Start()
    {

    }

    public void AddPuzzleImageURL()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        string inputUrl = link.text;

        // Extract the file ID from the input URL
        string fileId = ExtractFileIdFromUrl(inputUrl);

        // Create the Google Drive direct download link
        string downloadLink = $"https://drive.google.com/uc?export=download&id={fileId}";

        // Save the download link to the puzzleList
        myLevelData.levelData[^1].textureURL = downloadLink;

        // puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].textureURL = link.text;
        SaveDataToJson();
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
}
