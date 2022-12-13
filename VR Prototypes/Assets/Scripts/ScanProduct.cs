using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScanProduct : MonoBehaviour
{
    [SerializeField]
    private TMP_Text totalCost;
    [SerializeField]
    private TMP_Text warning;
  //  private float total;
    [SerializeField]
    private FloatSO total;
    [SerializeField]
    private Transform basketTransform;
    [SerializeField]
    private RecipeReader ingredients; //Para utilizar em 3D

    private bool isInFlag;

    // Start is called before the first frame update
    void Start()
    {
        total.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(total.Value > ingredients.budget)
        {
            warning.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objects"))
        {
            isInFlag = false;
            for (int i = 0; i < ingredients.myRecipeList.recipe[ingredients.randomIndex].ingredients.Length; i++)
            {
                if (other.name.Equals(ingredients.productsToGet[i].text))
                {
                    ObjectGrabbable objectGrabbable = other.GetComponent<ObjectGrabbable>();
                    objectGrabbable.Drop(); //just for 3d purpose

                    Product product = other.GetComponent<Product>();

                    total.Value += product.productCost; //add the products cost to total

                    totalCost.SetText("Total: " + total.Value.ToString());

                    other.transform.position = basketTransform.position;
                    isInFlag = true;
                }
            }

            if (!isInFlag)
            {
                Debug.Log("Produto errado..");
                isInFlag = false;
            }

        }
    }
}
