using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MBController : MonoBehaviour
{
    public RawImage introduceCard;
    public RawImage introduceCode;
    public RawImage optionsPanel;
    public RawImage moneyPanel;
    public RawImage RemoveCardPanel;

    //Para utilizar caso O MB esteja na mesma cena do supermercado
    //[SerializeField]
    //private ProductListManager code;
    public TextMeshProUGUI uiCode;
    public string code;

    private string insertedCode = null;
    public TextMeshProUGUI outputCode;
    public TextMeshProUGUI incorrectCode;
    private bool allowCode;

    [SerializeField]
    private bool moveCard;
    public Transform cardTransform;
    [SerializeField]
    private Transform cardPosition;
    [SerializeField]
    private Vector3 a;
    [SerializeField]
    private Vector3 b;
    public float speed;

    public Transform cardInitialPosition;

    //MoneyPosition
    public Transform moneyPosition;

    private bool moveCardBack;

    public HandGrabInteractor handGrabInteractorR;
    public HandGrabInteractor handGrabInteractorL;

    public AudioSource cardSound;
    public AudioSource codeAudio;
    public AudioSource grabMoneySound;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCode();
        uiCode.text = "Pin: " + code;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Raise10();
        }
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

        if (moveCardBack)
        {
            speed = (float)(speed + 0.002);
            cardPosition.position = Vector3.MoveTowards(a, b, speed);

            if (cardPosition.position == b)
            {
                moveCardBack = false;
                speed = (float)0.2;
                speed = 0;
            }
        }
    }

    public void GenerateCode()
    {
        // Instantiate random number generator 
        Random random = new Random();

        // Print 4 random numbers between 50 and 100 
        for (int i = 1; i <= 4; i++)
            code = code + random.Next(0, 9).ToString();
        Debug.Log(code);
    }

    public void CodeFunction(string numbers)
    {
        if (allowCode)
        {
            insertedCode = insertedCode + numbers;
            outputCode.text += "*";
        }
    }

    public void Enter()
    {
        //if (insertedCode == code.code)
        //{
        //    optionsPanel.gameObject.SetActive(true);
        //}
        //else
        //{
        //    incorrectCode.gameObject.SetActive(true);
        //}
        if (insertedCode == code)
        {
            optionsPanel.gameObject.SetActive(true);
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

    public void StopOperation()
    {
        moveCardBack = true;
        insertedCode = null;
        outputCode.text = insertedCode;
        introduceCode.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
        moneyPanel.gameObject.SetActive(false);
        RemoveCardPanel.gameObject.SetActive(false);
    }

    public void Raise10()
    {
        moveCardBack = true;
        Instantiate(Resources.Load("Money/10euros"), moneyPosition.position, Quaternion.Euler(0,90,90));
        grabMoneySound.Play();
    }

    public void Raise20()
    {
        moveCardBack = true;
        Instantiate(Resources.Load("Money/20euros"), moneyPosition.position, Quaternion.Euler(0, 90, 90));
        grabMoneySound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            cardSound.Play();
            codeAudio.PlayDelayed(0.2f);
            moveCard = true;
            introduceCode.gameObject.SetActive(true);
            allowCode = true;

            //HandGrabInteractable cardHandGrab = other.GetComponent<HandGrabInteractable>();
            //cardHandGrab.enabled = false;

            SnapInteractor snapInteractor = other.GetComponent<SnapInteractor>();
            snapInteractor.enabled = false;

            handGrabInteractorR.ForceRelease();
            handGrabInteractorL.ForceRelease();


            a = other.transform.position;
            b = cardTransform.position;

            cardPosition = other.transform;

           // other.transform.parent = transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (moveCardBack)
        {
            if (other.CompareTag("Card"))
            {
                cardSound.Play();
                a = other.transform.position;
                b = cardInitialPosition.position;

                cardPosition = other.transform;

                SnapInteractor snapInteractor = other.GetComponent<SnapInteractor>();
                snapInteractor.enabled = true;

                //HandGrabInteractable cardHandGrab = other.GetComponent<HandGrabInteractable>();
                //cardHandGrab.enabled = true;

                // other.transform.parent = transform;
                insertedCode = null;
                outputCode.text = insertedCode;
                introduceCode.gameObject.SetActive(false);
                optionsPanel.gameObject.SetActive(false);
                moneyPanel.gameObject.SetActive(false);
                RemoveCardPanel.gameObject.SetActive(false);
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Card"))
    //    {
    //        Debug.Log("exit" + Time.deltaTime);
            
    //    }
    //}
}
