using RoboRyanTron.Unite2017.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class PaintingMultiplayerGameManager : NetworkBehaviour
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
    public GameObject r_handMarker;
    public GameObject l_handMarker;
    [SerializeField]
    private GameEvent setWinPanel;
    private float startingTime = 5;
    [SerializeField]
    private NetworkVariable<float> currentTime = new NetworkVariable<float>(0);
    [SerializeField]
    private TextMeshProUGUI timer;
    private bool winFlag;
    private IKTargetFollowVRRig player;
    public GameObject guessCanvas;
    public TextMeshProUGUI[] buttonText;
    public TextMeshProUGUI tryAgainText;
    public GameObject marker;
    public GameObject[] handMarker;
    public GameObject eraser;
    public RawImage drawMiniCanvasImage;

    public int currentIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        draws = Resources.LoadAll<Sprite>("Draws/");
        player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();

        if (IsServer)
            currentTime.Value = startingTime;

        GetRandomDraws();
    }

    private void GetRandomDraws()
    {
        usedIndexes.Clear(); // Clear used indexes for new draws

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
        guessCanvas.SetActive(false);
        winningWord = drawBtn[buttonId].name;
        drawMiniCanvasImage.texture = drawBtn[buttonId].image.sprite.texture;
        SendWinningWordServerRpc(winningWord);
        PopulateGuessCanvasServerRpc();
        //charactersSet = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendWinningWordServerRpc(string clientWinningWord)
    {       
        SendWinningWordClientRpc(clientWinningWord);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerShipServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            foreach (GameObject handMarkers in handMarker)
            {
                Marker handMarkerOwner = handMarkers.GetComponent<Marker>();
                handMarkerOwner.SetClientHandMarkerOwnershipServerRPC();
            }

            SetNetworkObjectOwner markerOwner = marker.GetComponent<SetNetworkObjectOwner>();//change the owner of the marker to let the client write with it
            markerOwner.SetClientOwnershipServerRPC();

            SetNetworkObjectOwner eraserOwner = eraser.GetComponent<SetNetworkObjectOwner>();//change the owner of the marker to let the client write with it
            eraserOwner.SetClientOwnershipServerRPC();
        }
        else
        {
            Debug.Log("not working");
        }

    }

    [ClientRpc]
    public void SendWinningWordClientRpc(string clientWinningWord)
    {
        winningWord = clientWinningWord;
        Debug.Log(winningWord);
    }

    [ServerRpc(RequireOwnership =false)]
    public void PopulateGuessCanvasServerRpc()
    {
        charactersSet = true;
        PopulateGuessCanvasClientRpc();
    }

    [ClientRpc]
    public void PopulateGuessCanvasClientRpc()
    {
        //ShuffleArray(buttonText);
        //ShuffleArray(draws);

        // Get a random index to place the winning word
        int winningWordIndex = random.Next(0, buttonText.Length);

        // Place the winning word at the random index
        buttonText[winningWordIndex].text = winningWord;

        // Loop through buttonText to populate other words
        int drawIndex = 0; // Index to track current position in draws array
        for (int i = 0; i < buttonText.Length; i++)
        {
            // Skip the index with the winning word
            if (i == winningWordIndex)
                continue;

            // Ensure we don't place the winning word again
            while (draws[drawIndex].name == winningWord)
            {
                drawIndex++;
            }

            buttonText[i].text = draws[drawIndex].name;
            drawIndex++;
        }
    }

    //[ClientRpc]
    //public void PopulateGuessCanvasClientRpc()
    //{
    //    buttonText[0].text = winningWord;

    //    for(int i = 1; i< buttonText.Length; i++)
    //    {
    //        if (draws[i].name != winningWord)
    //            buttonText[i].text = draws[i].name;
    //        else
    //            i--;
    //    }
    //}

    [ClientRpc]
    public void SetCharactersInCanvasClientRPC()
    {
        for (int i = 0; i < winningWord.Length; i++)
        {
            Debug.Log(i);
            canvasWord.SetText(canvasWord.text + "_");
           // StartCoroutine(RevealLettersOverTime());
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

    public void RevealNextLetterTherapist()
    {
        RevealNextLetterClientRpc();
    }

    //To let the therapist be responsible for revealing the letters
    [ClientRpc]
    public void RevealNextLetterClientRpc()
    {
        if (currentIndex < winningWord.Length)
        {
            char[] currentTextArray = canvasWord.text.ToCharArray();
            currentTextArray[currentIndex] = winningWord[currentIndex];
            canvasWord.text = new string(currentTextArray);
            currentIndex++;
        }
    }

    public static void ShuffleArray<T>(T[] array)
    {
        Random random = new Random();

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            // Swap array[i] and array[j]
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
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

    public void GuessDraw(int id)
    {
        string userGuess = buttonText[id].text;
        userGuess.Trim();
        string correctWord = winningWord.Trim();

        // Log the lengths and characters of the strings for debugging
        Debug.Log($"User Guess: '{userGuess}' (Length: {userGuess.Length})");
        Debug.Log($"Correct Word: '{correctWord}' (Length: {correctWord.Length})");

        if (string.Equals(userGuess, correctWord, StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("GAnhou!");
            GuessDrawServerRpc();
            //RaiseWinPanelClientRPC();
        }
        else
        {
            Debug.Log("tryAgain!");
            tryAgainText.gameObject.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership =false)]
    public void GuessDrawServerRpc(ServerRpcParams serverRpcParams = default)
    {
        //var clientId = serverRpcParams.Receive.SenderClientId;
        //Debug.Log(clientId);

        //foreach (GameObject handMarkers in handMarker)
        //{
        //    Marker handMarkerOwner = handMarkers.GetComponent<Marker>();
        //    handMarkerOwner.SetClientHandMarkerOwnershipServerRPC();
        //}

        //SetNetworkObjectOwner markerOwner = marker.GetComponent<SetNetworkObjectOwner>();//change the owner of the marker to let the client write with it
        //markerOwner.SetClientOwnershipServerRPC();

        //SetNetworkObjectOwner eraserOwner = eraser.GetComponent<SetNetworkObjectOwner>();//change the owner of the marker to let the client write with it
        //eraserOwner.SetClientOwnershipServerRPC();
        //if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        //{
        //    var client = NetworkManager.ConnectedClients[clientId];
        //    Marker markerOwner = marker.GetComponent<Marker>();
        //    markerOwner.SetClientOwnershipServerRPC();
        //    //marker.GetComponent<NetworkObject>().ChangeOwnership(clientId);
        //    Debug.Log("Client is now the owner");
        //}
        //else
        //{
        //    Debug.Log("not working");
        //}

        winFlag = true;
        RaiseWinPanelClientRPC();
    }

    [ClientRpc]
    public void RaiseWinPanelClientRPC()
    {
        if (player.currentPaintPosition >= player.spawnLocationsNumber - 1)
            player.currentPaintPosition = 0;
        else
            player.currentPaintPosition++;
        
        setWinPanel.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
            return;

        if (charactersSet)
        {
            SetCharactersInCanvasClientRPC();
            charactersSet = false;
        }

        if (winFlag)
        {
            currentTime.Value -= 1 * Time.deltaTime;
            if (currentTime.Value <= 0)
            {
                NextLevelServerRPC();
            }
            timer.SetText("Proximo nivel em " + currentTime.Value.ToString("0"));
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC()
    {
        NetworkManager.SceneManager.LoadScene("PaintingLevels2", LoadSceneMode.Single);
    }

    public void SelectLeftHandMarker()
    {
        SelectLeftHandMarkerServerRpc();
        //ActivateleftHandMarkerClientRpc();
    }

    public void SelectRightHandMarker()
    {
        SelectRightHandMarkerServerRpc();
        //ActivateRightHandMarkerClientRpc();
    }

    public void SelectMarker()
    {
        SelectRightHandMarkerServerRpc();
        //ActivateRightHandMarkerClientRpc();

        SelectLeftHandMarkerServerRpc();
        //ActivateleftHandMarkerClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectLeftHandMarkerServerRpc()
    {
        //r_handMarker.SetActive(false);
        ActivateleftHandMarkerClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SelectRightHandMarkerServerRpc()
    {
        ActivateRightHandMarkerClientRpc();
        //l_handMarker.SetActive(false);
    }

    [ClientRpc]
    public void ActivateleftHandMarkerClientRpc()
    {
        r_handMarker.SetActive(false);
    }

    [ClientRpc]
    public void ActivateRightHandMarkerClientRpc()
    {
        l_handMarker.SetActive(false);
        Debug.Log("HandActivated");
    }
}
