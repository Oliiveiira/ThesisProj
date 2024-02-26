using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticLevelsBasketCounterMultiplayer : NetworkBehaviour
{
    [SerializeField]
    private StaticListManagerMultiplayer products;

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

    [SerializeField]
    private GameObject registerBox;

    //[SerializeField]
    //private GameObject wallet;

    //Flag to stop Comparing the products name, in order to reset the products in the basket in the end of the level
    private bool stopComparing;

    [SerializeField]
    private IntSO level;

    public string nextSceneName;
    public TextMeshProUGUI timer;
    private float startingTime = 5;
    [SerializeField]
    private NetworkVariable<float> currentTime = new NetworkVariable<float>(0);

    private void Start()
    {
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
        if (successCounter == products.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count)
        {
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
        NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
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
                for (int i = 0; i < products.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
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
                    product.SetProductInitialPosition();
                }

                Win();
            }
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
                for (int i = 0; i < products.mystaticLevelsLists.recipe[level.Value].ingredientsName.Count; i++)
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
