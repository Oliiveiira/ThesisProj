using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomPuzzleDifficultyValue : LevelsDataWritter
{
    [SerializeField]
    private TextMeshProUGUI sliderText;
    [SerializeField]
    private Slider slider;

    private void Start()
    {

    }

    public void SliderChange(float value)
    {
        sliderText.text = value.ToString("0.000");
    }

    public void AddDifficultyValue()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);

        myLevelData.levelData[^1].difficultyValue = slider.value;
        SaveDataToJson();
    }

}
