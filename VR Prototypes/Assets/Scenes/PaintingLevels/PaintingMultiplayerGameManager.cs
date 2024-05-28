using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PaintingMultiplayerGameManager : MonoBehaviour
{
    //TODO
    //btn name will be used to compare with the answers from the other players
    //A way to let the other players write their guess

    public Sprite[] draws;
    public Button[] drawBtn;
    private static Random random = new Random();
    [SerializeField]
    private List<int> usedIndexes;
    public string winningWord;
    public TextMeshProUGUI guessText;
    public TextMeshProUGUI canvasWord;
    public bool charactersSet;
    public float revealInterval = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        draws = Resources.LoadAll<Sprite>("Draws/");
        GetRandomDraws();
    }

    private void GetRandomDraws()
    {
        for (int i = 0; i < drawBtn.Length; i++)
        {
            // Get the count of pair paths for the current index

            int randomdrawIndex = random.Next(0, draws.Length);

            do
            {
                randomdrawIndex = random.Next(0, draws.Length);
            } while (usedIndexes.Contains(randomdrawIndex)); // Check if the index has been used before

            // GameObject pairsToInstantiate = Resources.Load<GameObject>(myPairsList.pairlevel[randomPairIndex].pairPath);
            drawBtn[i].image.sprite = draws[randomdrawIndex];
            drawBtn[i].name = draws[randomdrawIndex].name;
            usedIndexes.Add(randomdrawIndex);
        }
    }

    public void PickDraw(int buttonId)
    {
        winningWord = drawBtn[buttonId].name;
        charactersSet = true;
    }


    public void SetCharactersInCanvas()
    {
        for (int i = 0; i < winningWord.Length; i++)
        {
            Debug.Log(i);
            canvasWord.SetText(canvasWord.text + "_");
            StartCoroutine(RevealLettersOverTime());
        }
    }

    private IEnumerator RevealLettersOverTime()
    {
        // Create a list of indices and shuffle it
        List<int> indices = new List<int>();
        for (int i = 0; i < winningWord.Length; i++)
        {
            indices.Add(i);
        }
        indices = ShuffleList(indices);

        // Reveal letters one by one at intervals
        for (int i = 0; i < indices.Count; i++)
        {
            yield return new WaitForSeconds(revealInterval);

            char[] currentTextArray = canvasWord.text.ToCharArray();
            currentTextArray[i] = winningWord[i];
            canvasWord.text = new string(currentTextArray);
        }
    }

    private List<int> ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = random.Next(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    public void GuessDraw(string userGuess)
    {
        userGuess.Trim();
        string correctWord = winningWord.Trim();

        // Log the lengths and characters of the strings for debugging
        Debug.Log($"User Guess: '{userGuess}' (Length: {userGuess.Length})");
        Debug.Log($"Correct Word: '{correctWord}' (Length: {correctWord.Length})");

        if (string.Equals(userGuess, correctWord, StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("GAnhou!");
        }
        else
        {
            Debug.Log("tryAgain!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (charactersSet)
        {
            SetCharactersInCanvas();
            charactersSet = false;
        }
        
    }
}
