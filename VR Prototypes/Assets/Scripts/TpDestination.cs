using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpDestination : MonoBehaviour
{
    private MeshRenderer mesh;
    public Transform player;
    public float targetRotation;
    [SerializeField]
    private GameEvent activateCart;
    //public float rotationLerpSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RotatePlayer(float duration, Quaternion targetRotation)
    {
        float timeElapsed = 0f;
        Quaternion initialRotation = player.rotation;

        while (timeElapsed < duration)
        {
            player.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        player.rotation = targetRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.enabled = false;
            player.rotation = Quaternion.Euler(0, targetRotation, 0);
            activateCart.Raise();
            //player.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0, targetRotation, 0), 10f);
            //player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, targetRotation, 0), Time.deltaTime * rotationLerpSpeed);
            //StartCoroutine(RotatePlayer(rotationLerpSpeed, Quaternion.Euler(0,targetRotation,0)));
        }
    }

    public void ActivateTPPoint()
    {
        mesh.enabled = true;
    }
}
