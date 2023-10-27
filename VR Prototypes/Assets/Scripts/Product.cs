using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Product : MonoBehaviour
{
    [SerializeField]
    private ObjectSO objects; //ScriptableObject with the product data
    public float productCost;
    [SerializeField]
    private TMP_Text costText;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool isGround;
    public bool isInBasket;
    public bool isInCart;
    public AudioSource sound;

    // Start is called before the first frame update
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        name = objects.productName;
        productCost = objects.productCost;
        costText.SetText(objects.productCost.ToString() + "€");
    }

    private void Update()
    {
        if (transform.position.y <= 0.2 && isGround && !isInBasket && !isInCart)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            isGround = false;

        }
        else if (transform.position.y <= 0.2 && isGround && isInCart || transform.position.y <= 0.2 && isGround && isInBasket)
        {
            transform.position = transform.parent.position;
            isGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = true;
        }
        //if (collision.collider.CompareTag("Cart") && !isInCart)
        //{
        //    //sound.Play();
        //    isInCart = true;
        //}
    }

    public void SetProductInitialPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
