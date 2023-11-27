using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddPuzzleDifficultyValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI sliderText;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private PuzzleListWritter puzzleList;
    [SerializeField]
    private IntSO puzzleLevelSO;

    public void SliderChange(float value)
    {
        sliderText.text = value.ToString("0.000");
    }

    public void AddDifficultyValue()
    {
        puzzleList.myPuzzleList.puzzlelevel[puzzleLevelSO.Value].difficultyValue = slider.value;
        puzzleList.SavePuzzleListToJson();
    }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }
}
