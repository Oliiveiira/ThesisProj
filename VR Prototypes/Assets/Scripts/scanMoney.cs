using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scanMoney : MonoBehaviour
{
    private bool isInFlag;
    public Transform moneyTransform;
    [SerializeField]
    private TMP_Text totalCost;
    [SerializeField]
    private TMP_Text change;
    [SerializeField]
    private ScanProduct products; //to get the products total
    [SerializeField]
    private FloatSO total;

    [SerializeField]
    private bool moveMoney;
    [SerializeField]
    private Transform moneyPosition;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(total.Value < 0)
        {
            total.Value *= -1;
            change.SetText("Troco: " + total.Value.ToString());
            total.Value = 0;
            totalCost.SetText("Total: " + total.Value.ToString());
        }
    }

    private void FixedUpdate()
    {
        if (moveMoney)
        {
            speed = (float)(speed + 0.002);
            moneyPosition.position = Vector3.MoveTowards(a, b, speed);

            if(moneyPosition.position == b)
            {
                moveMoney = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            moveMoney = true;
            //Animator anim = other.GetComponent<Animator>();
            //anim.gameObject.SetActive(true);
            //anim.Play("MoneyAnim");

            ObjectGrabbable objectGrabbable = other.GetComponent<ObjectGrabbable>();
            objectGrabbable.Drop();

            Money money = other.GetComponent<Money>();
            total.Value -= money.value;

            a = other.transform.position;
            b = moneyTransform.position;

            moneyPosition = other.transform;
            //other.transform.position = Vector3.Lerp(a, b, speed);
            //other.transform.position = moneyTransform.position;

            totalCost.SetText("Total: " + total.Value.ToString());
            
            //isInFlag = false;
            //for (int i = 0; i < ingredients.myRecipeList.recipe[ingredients.randomIndex].ingredients.Length; i++)
            //{
            //    if (other.name.Equals(ingredients.productsToGet[i].text))
            //    {
            //        Product product = other.GetComponent<Product>();

            //        total += product.productCost;

            //        totalCost.SetText("Total: " + total.ToString());

            //        other.transform.position = basketTransform.position;
            //        isInFlag = true;
            //    }
            //}

            //if (!isInFlag)
            //{
            //    Debug.Log("Produto errado..");
            //    isInFlag = false;
            //}

        }
    }
}
