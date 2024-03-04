using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class ProductFindManager : NetworkBehaviour
{
    public List<GameObject> allProducts;
    public List<GameObject> randomProducts;
    public int numberOfProducts;

    public List<Transform> shelfTransforms;
    public List<Transform> tablePositions;
    public List<GameObject> spawnedShelfProducts;

    public List<GameObject> leftProducts;
    public List<GameObject> rightProducts;

    public Material transparentMaterial;
    public int randomIndex;

    public AudioSource popSound;
    public NetworkVariable<bool> isInLeftPlace;
    public NetworkVariable <bool> isInRightPlace;
    public bool canCompare;

    public List<Material> productMaterials;

    [SerializeField]
    private GameEvent setWinPanel;
    public string nextSceneName;
    private bool winFlag;

    //Timer
    private float startingTime = 5;
    [SerializeField]
    private NetworkVariable<float> currentTime = new NetworkVariable<float>(0);
    [SerializeField]
    private TextMeshProUGUI timer;

    // Start is called before the first frame update
    void Start()
    {
        allProducts = new List<GameObject>(Resources.LoadAll<GameObject>("NetworkProducts/"));
        if(IsServer)
        {
            GetRandomProducts();
            SpawnTableTransforms();
            SpawnProductToGet();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer && canCompare)
            ComparePositions();

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

    public void GetRandomProducts()
    {
        for(int i = 0; i < numberOfProducts; i++)
        {
            Random.seed = System.DateTime.Now.Millisecond;
            int randomIndex = Random.Range(0, allProducts.Count);
            randomProducts.Add(allProducts[randomIndex]);
            allProducts.RemoveAt(randomIndex);
        }
    }

    public void SpawnTableTransforms()
    {
       int i = 0;
       foreach(GameObject product in randomProducts)
       {
            GameObject leftProduct = Instantiate(product, tablePositions[i].position, product.transform.rotation);
            NetworkObject leftProductNetwork = leftProduct.GetComponent<NetworkObject>();
            leftProductNetwork.SpawnWithOwnership(0, destroyWithScene: true);
            leftProducts.Add(leftProduct);

            GameObject rightProduct = Instantiate(product, tablePositions[i + tablePositions.Count / 2].position, product.transform.rotation);
            NetworkObject rightProductNetwork = rightProduct.GetComponent<NetworkObject>();
            rightProductNetwork.SpawnWithOwnership(1, destroyWithScene: true); // TROCAR OWNERSHIP
            rightProducts.Add(rightProduct); 
            i++;
       }
    }

    public void SpawnProductToGet()
    {
        if (randomProducts.Count <= 0)
        {
            winFlag = true;
            RaiseWinPanelClientRPC();
           // setWinPanel.Raise();
            Debug.Log("Win");
            return;
        }

        randomIndex = Random.Range(0, randomProducts.Count);

        for(int i = 0; i < shelfTransforms.Count; i++)
        {
            GameObject productToPick = Instantiate(randomProducts[randomIndex], shelfTransforms[i].transform.position, randomProducts[randomIndex].transform.rotation);
            productToPick.transform.localScale *= 0.99f;
            NetworkObject networkProduct = productToPick.GetComponent<NetworkObject>();
            networkProduct.Spawn(destroyWithScene: true);
            //Renderer productMaterial = productToPick.GetComponent<Renderer>();
            NetworkObjectReference productReference = new NetworkObjectReference(productToPick.GetComponent<NetworkObject>());
            DisableHandGrabClientRpc(productReference);

            if (i % 2 == 0)
            {
                SetProductMaterialClientRpc(productReference);
            }
            spawnedShelfProducts.Add(productToPick);
        }
        canCompare = true;
    }

    [ClientRpc]
    public void DisableHandGrabClientRpc(NetworkObjectReference productReference)
    {
        NetworkObject productObject;
        if (productReference.TryGet(out productObject))
        {
            HandGrabInteractable handGrabInteractable = productObject.GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
        }
    }

    [ClientRpc]
    public void SetProductMaterialClientRpc(NetworkObjectReference productReference)
    {
        NetworkObject productObject;
        if (productReference.TryGet(out productObject))
        {
            productMaterials = productObject.GetComponent<Renderer>().materials.ToList();
            for (int j = 0; j < productMaterials.Count; j++)
            {
                productMaterials[j].shader = transparentMaterial.shader;
                productMaterials[j].CopyPropertiesFromMaterial(transparentMaterial);
            }
        }    
    }


    public void ComparePositions()
    {
        if (Vector3.Distance(leftProducts[randomIndex].transform.position, shelfTransforms[0].position) < 0.15f && !isInLeftPlace.Value)
        {
            leftProducts[randomIndex].transform.SetPositionAndRotation(spawnedShelfProducts[0].transform.position, spawnedShelfProducts[0].transform.rotation);
            Rigidbody leftProductRb = leftProducts[randomIndex].GetComponent<Rigidbody>();
           // leftProductRb.isKinematic = false;

            Debug.Log("isHere");
            popSound.Play();
            HandGrabInteractable handGrabInteractable = leftProducts[randomIndex].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
            isInLeftPlace.Value = true;

            NetworkBehaviourReference leftProductReference = new NetworkBehaviourReference(leftProducts[randomIndex].GetComponent<NetworkBehaviour>());
            Debug.Log(leftProductReference);
            DisableLeftInteractableClientRpc(leftProductReference);
        }
        else if (Vector3.Distance(rightProducts[randomIndex].transform.position, shelfTransforms[2].position) < 0.15f && !isInRightPlace.Value)
        {
            rightProducts[randomIndex].transform.SetPositionAndRotation(spawnedShelfProducts[2].transform.position, spawnedShelfProducts[0].transform.rotation);
            Rigidbody rightProductRb = rightProducts[randomIndex].GetComponent<Rigidbody>();
           // rightProductRb.isKinematic = false;

            popSound.Play();
            HandGrabInteractable handGrabInteractable = rightProducts[randomIndex].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
            isInRightPlace.Value = true;

            NetworkBehaviourReference rightProductReference = new NetworkBehaviourReference(rightProducts[randomIndex].GetComponent<NetworkBehaviour>());
            Debug.Log(rightProductReference);
            DisableRightInteractableClientRpc(rightProductReference);
        }
        else if(isInLeftPlace.Value && isInRightPlace.Value)
        {
            isInLeftPlace.Value = false;
            isInRightPlace.Value = false;
            canCompare = false;
            StartCoroutine(DeactivatePair(randomIndex));
        }
    }

    [ClientRpc]
    private void DisableLeftInteractableClientRpc(NetworkBehaviourReference leftproductReference)
    {
        popSound.Play();
        NetworkBehaviour leftProductBehaviour;
        if (leftproductReference.TryGet<NetworkBehaviour>(out leftProductBehaviour))
        {
            HandGrabInteractable handGrabInteractable = leftProductBehaviour.GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
            if (leftProductBehaviour.IsOwner)
            {
                leftProductBehaviour.transform.SetPositionAndRotation(spawnedShelfProducts[0].transform.position, spawnedShelfProducts[0].transform.rotation);
            }
        }
    }

    [ClientRpc]
    private void DisableRightInteractableClientRpc(NetworkBehaviourReference rightproductReference)
    {
        popSound.Play();
        NetworkBehaviour rightProductBehaviour;
        if (rightproductReference.TryGet<NetworkBehaviour>(out rightProductBehaviour))
        {
            HandGrabInteractable handGrabInteractable = rightProductBehaviour.GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
            if (rightProductBehaviour.IsOwner)
            {
                rightProductBehaviour.transform.SetPositionAndRotation(spawnedShelfProducts[2].transform.position, spawnedShelfProducts[0].transform.rotation);
            }
        }
    }

    IEnumerator DeactivatePair(int lastRandomIndex)
    {
        yield return new WaitForSeconds(2);
        //leftProducts[lastRandomIndex].SetActive(false);
        //rightProducts[lastRandomIndex].SetActive(false);
        //randomProducts[lastRandomIndex].SetActive(false);
        Destroy(leftProducts[lastRandomIndex]);
        Destroy(rightProducts[lastRandomIndex]);
        Destroy(randomProducts[lastRandomIndex]);
        foreach (GameObject product in spawnedShelfProducts)
            Destroy(product);
        leftProducts.RemoveAt(lastRandomIndex);
        rightProducts.RemoveAt(lastRandomIndex);
        randomProducts.RemoveAt(lastRandomIndex);
        spawnedShelfProducts.RemoveAll(spawnedShelfProducts => spawnedShelfProducts);
        SpawnProductToGet();
    }

    [ClientRpc]
    public void RaiseWinPanelClientRPC()
    {
        setWinPanel.Raise();
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextLevelServerRPC()
    {
        NetworkManager.SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
}
