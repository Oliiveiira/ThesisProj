using Oculus.Interaction.HandGrab;
using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScanCard : MonoBehaviour
{
    // public GameObject passcodePanel;
    public TextMeshPro introduceCode;
    private string insertedCode = null;
    public TextMeshPro outputCode;
    [SerializeField]
    private GameManager code;

    [SerializeField]
    private GameEvent setWinPanel;

    [SerializeField]
    private GameEvent releaseHandPoke;

    public TextMeshPro incorrectCode;

    public GameObject cashRegister;

    public TMP_Text warning;

    // [SerializeField]
    public ScanProduct products; //to get the products total
    [SerializeField]
    private bool moveCard;
    [SerializeField]
    private Transform cardPosition;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;

    public Transform cardTransform;
    public AudioSource cardSound;//trigger the sound
    public AudioSource codeSound;//trigger the sound

    public float speed;

    private bool allowCode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (moveCard)
        {
            speed = (float)(speed + 0.002);
            cardPosition.position = Vector3.MoveTowards(a, b, speed);

            if (cardPosition.position == b)
            {
                moveCard = false;
                speed = (float)0.2;
                speed = 0;
            }
        }
    }

    public void CodeFunction(string numbers)
    {
        if (allowCode)
        {
            insertedCode = insertedCode + numbers;
            outputCode.text = insertedCode;
        }
    }

    public void Enter()
    {
        if (insertedCode == code.code)
        {
            releaseHandPoke.Raise();
            cashRegister.SetActive(false);
            setWinPanel.Raise();
        }
        else
        {
            incorrectCode.gameObject.SetActive(true);
        }
    }

    public void Delete()
    {
        incorrectCode.gameObject.SetActive(false);
        insertedCode = null;
        outputCode.text = insertedCode;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card") /*&& products.paymentAvailable*/)
        {
            if (products.paymentAvailable)
            {
                allowCode = true;
                introduceCode.gameObject.SetActive(true);
                cardSound.Play();
                codeSound.PlayDelayed(0.2f);
                moveCard = true;

                HandGrabInteractable cardHandGrab = other.GetComponent<HandGrabInteractable>();
                cardHandGrab.enabled = false;

                a = other.transform.position;
                b = cardTransform.position;

                cardPosition = other.transform;

                other.transform.parent = transform;
            }
            else
            {
                warning.gameObject.SetActive(true);
            }

        }
    }
}
