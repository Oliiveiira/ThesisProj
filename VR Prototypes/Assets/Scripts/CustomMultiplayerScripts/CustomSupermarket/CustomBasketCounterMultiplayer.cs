using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomBasketCounterMultiplayer : CustomLevelsData
{
    [SerializeField]
    private CustomSupermarketMultiplayer products;
    private CustomMultiplayerSceneManager multiplayerSceneManager;
    private GetErrorCountData errorCountData;

    private int successCounter;
    [SerializeField]
    private bool isInFlag;
    [SerializeField]
    private GameEvent setWinPanel;

    [SerializeField]
    private AudioSource correctSound;
    [SerializeField]
    private AudioSource wrongSound;

    [SerializeField]
    private GameObject[] shelves;

    //[SerializeField]
    //private RecipeReader listProduct;

    private Product product;

    //Flag to stop Comparing the products name, in order to reset the products in the basket in the end of the level
    private bool stopComparing;

    public string nextSceneName;
    public TextMeshProUGUI timer;
    private float startingTime = 5;
    [SerializeField]
    private NetworkVariable<float> currentTime = new NetworkVariable<float>(0);
    private bool listPositionRemoved;
    public int numberOfErrors;

    private void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
        multiplayerSceneManager = GetComponent<CustomMultiplayerSceneManager>();
        errorCountData = GetComponent<GetErrorCountData>();

        if (IsServer)
            currentTime.Value = startingTime;
    }

    private void Update()
    {
        // PlayAudio();
        //Win();
        if (stopComparing)
        {
            if (IsServer)
            {
                currentTime.Value -= 1 * Time.deltaTime;
                if (currentTime.Value <= 0)
                {
                    NextLevelServerRPC();
                }
            }
            timer.SetText("Proximo nivel em " + currentTime.Value.ToString("0"));
        }
    }

    void Win()
    {
        if (successCounter == myLevelData.levelData[0].ingredientsPath.Count)
        {
            errorCountData.SaveData(numberOfErrors);
            DeactivateShelves();
            // registerBox.SetActive(true);
            // wallet.SetActive(true);
            stopComparing = true;
            setWinPanel.Raise();
            WinClientRPC();
            Debug.Log("Ganhou");
        }
    }

    [ClientRpc]
    public void WinClientRPC()
    {
        DeactivateShelves();
        stopComparing = true;
        setWinPanel.Raise();
    }

    [ClientRpc]
    public void ActivateSoundClientRPC()
    {
        correctSound.Play();
    }

    void DeactivateShelves()
    {
        foreach (GameObject obj in shelves)
        {
            obj.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC()
    {
        //string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        //string jsonText = File.ReadAllText(jsonFilePath);
        //myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
        //if (!listPositionRemoved)
        //{
        //    RemoveDataAtIndex(0);
        //    listPositionRemoved = true;
        //}
        //if (myLevelData.levelData.Count > 0)
        //    nextSceneName = myLevelData.levelData[0].level;
        //else
        //    nextSceneName = "CustomMultiplayerHub";

        //NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        multiplayerSceneManager.NextLevelServerRPC();
    }

    //private ObjectGrabbable grabbedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        if (!stopComparing)
        {
            if (other.CompareTag("Objects"))
            {
                //ObjectSO products = other.gameObject.GetComponent<ObjectSO>();
                product = other.GetComponent<Product>();
                isInFlag = false;
                for (int i = 0; i < myLevelData.levelData[0].ingredientsName.Count; i++)
                {
                    if (other.name.Equals(products.productsToGet[i].text))
                    {
                        other.transform.SetParent(this.transform);
                        product.isInBasket = true;
                        Debug.Log("yes");
                        successCounter++;
                        isInFlag = true;
                        correctSound.Play();
                        ActivateSoundClientRPC();
                        break;
                    }
                }

                if (!product.isInBasket)
                {
                    numberOfErrors++;
                    NetworkObjectReference productReference = new NetworkObjectReference(product.GetComponent<NetworkObject>());
                    product.SetProductInitialPosition();
                    SetProductToInitialPositionClientRPC(productReference);
                }

                Win();
            }
        }
    }

    [ClientRpc]
    public void SetProductToInitialPositionClientRPC(NetworkObjectReference productReference)
    {
        NetworkObject productObject;
        if (productReference.TryGet(out productObject))
        {
            productObject.GetComponent<Product>().SetProductInitialPosition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer)
            return;
        if (!stopComparing)
        {
            if (other.CompareTag("Objects"))
            {
                for (int i = 0; i < myLevelData.levelData[0].ingredientsName.Count; i++)
                {
                    if (other.name.Equals(products.productsToGet[i].text))
                    {
                        other.transform.parent = null;
                        successCounter--;
                        product.isInBasket = false;
                    }
                }
            }
        }
    }
}