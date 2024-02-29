using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

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
    public bool isInLeftPlace;
    public bool isInRightPlace;
    public bool canCompare;

    public List<Material> productMaterials;

    [SerializeField]
    private GameEvent setWinPanel;
    // Start is called before the first frame update
    void Start()
    {
        allProducts = new List<GameObject>(Resources.LoadAll<GameObject>("NetworkProducts/"));
        GetRandomProducts();
        SpawnTableTransforms();
        SpawnProductToGet();
    }

    // Update is called once per frame
    void Update()
    {
        if(canCompare)
            ComparePositions();
    }

    public void GetRandomProducts()
    {
        for(int i = 0; i < numberOfProducts; i++)
        {
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
            leftProducts.Add(leftProduct);

            GameObject rightProduct = Instantiate(product, tablePositions[i + tablePositions.Count / 2].position, product.transform.rotation);
            rightProducts.Add(rightProduct);
            i++;
       }
    }

    public void SpawnProductToGet()
    {
        if (randomProducts.Count <= 0)
        {
            setWinPanel.Raise();
            Debug.Log("Win");
            return;
        }

        randomIndex = Random.Range(0, randomProducts.Count);

        for(int i = 0; i < shelfTransforms.Count; i++)
        {
            GameObject productToPick = Instantiate(randomProducts[randomIndex], shelfTransforms[i].transform.position, randomProducts[randomIndex].transform.rotation);
            Renderer productMaterial = productToPick.GetComponent<Renderer>();
            productMaterials = productToPick.GetComponent<Renderer>().materials.ToList();
            if (i % 2 == 0)
            {
                for (int j = 0; j < productMaterials.Count; j++)
                {
                    productMaterials[j].shader = transparentMaterial.shader;
                    productMaterials[j].CopyPropertiesFromMaterial(transparentMaterial);
                }
            }
            spawnedShelfProducts.Add(productToPick);
        }
        canCompare = true;
    }

    public void ComparePositions()
    {
        if (Vector3.Distance(leftProducts[randomIndex].transform.position, shelfTransforms[0].position) < 0.1f && !isInLeftPlace)
        {
            leftProducts[randomIndex].transform.position = shelfTransforms[0].position;
            Debug.Log("isHere");
            popSound.Play();
            HandGrabInteractable handGrabInteractable = leftProducts[randomIndex].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
            isInLeftPlace = true;
        }
        else if (Vector3.Distance(rightProducts[randomIndex].transform.position, shelfTransforms[2].position) < 0.1f && !isInRightPlace)
        {
            rightProducts[randomIndex].transform.position = shelfTransforms[2].position;

            popSound.Play();
            HandGrabInteractable handGrabInteractable = rightProducts[randomIndex].GetComponent<HandGrabInteractable>();
            if (handGrabInteractable != null)
            {
                handGrabInteractable.enabled = false;
            }
            isInRightPlace = true;
        }
        else if(isInLeftPlace && isInRightPlace)
        {
            isInLeftPlace = false;
            isInRightPlace = false;
            canCompare = false;
            StartCoroutine(DeactivatePair(randomIndex));
        }
    }


    IEnumerator DeactivatePair(int lastRandomIndex)
    {
        yield return new WaitForSeconds(2);
        leftProducts[lastRandomIndex].SetActive(false);
        rightProducts[lastRandomIndex].SetActive(false);
        randomProducts[lastRandomIndex].SetActive(false);
        foreach (GameObject product in spawnedShelfProducts)
            Destroy(product);
        leftProducts.RemoveAt(lastRandomIndex);
        rightProducts.RemoveAt(lastRandomIndex);
        randomProducts.RemoveAt(lastRandomIndex);
        spawnedShelfProducts.RemoveAll(spawnedShelfProducts => spawnedShelfProducts);
        SpawnProductToGet();
    }
}
