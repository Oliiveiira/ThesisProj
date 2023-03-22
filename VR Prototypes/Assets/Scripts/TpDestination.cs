using RoboRyanTron.Unite2017.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpDestination : MonoBehaviour
{
    private MeshRenderer mesh;
    public Transform player;
    public float targetRotation;
    public float cartTargetRotation;
    public float walletTargetRotation;
    [SerializeField]
    private GameEvent activateCart;
    [SerializeField]
    private GameEvent activateWallet;

    public Transform cart;
    public float cartPositionX;
    public float cartPositionZ;

    public Transform wallet;
    public float walletPositionX;
    public float walletPositionZ;

    private Collider capsuleCollider;


    //public float rotationLerpSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        capsuleCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RotatePlayer(float duration, Quaternion targetRotation)
    {
        float timeElapsed = 0f;
        Quaternion initialRotation = player.rotation;

        while (timeElapsed < duration)
        {
            player.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / duration);
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
            //  player.rotation = Quaternion.Euler(0, targetRotation, 0);
            ChangeCartPosition();
            ChangeWalletPosition();
            activateCart.Raise();
            activateWallet.Raise();
            //player.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0, targetRotation, 0), 10f);
            //player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, targetRotation, 0), Time.deltaTime * rotationLerpSpeed);
            //StartCoroutine(RotatePlayer(rotationLerpSpeed, Quaternion.Euler(0,targetRotation,0)));
        }
    }

    public void ActivateTPPointMesh()
    {
        mesh.enabled = true;
    }

    public void ChangeCartPosition()
    {
        //cart.transform.SetPositionAndRotation(new Vector3(cartPositionX, cart.position.y, cartPositionZ), Quaternion.Euler(cart.rotation.x, cartTargetRotation, cart.rotation.z));
        cart.localRotation = Quaternion.Euler(cart.localRotation.x, cartTargetRotation, cart.localRotation.z);
        cart.localPosition = new Vector3(cartPositionX, cart.localPosition.y, cartPositionZ);
    }

    public void ChangeWalletPosition()
    {
        //cart.transform.SetPositionAndRotation(new Vector3(cartPositionX, cart.position.y, cartPositionZ), Quaternion.Euler(cart.rotation.x, cartTargetRotation, cart.rotation.z));
        wallet.localRotation = Quaternion.Euler(wallet.localRotation.x, walletTargetRotation, wallet.localRotation.z);
        wallet.localPosition = new Vector3(walletPositionX, wallet.localPosition.y, walletPositionZ);
    }

    //public void DeactivateTeleportPoint()
    //{
    //    capsuleCollider.enabled = false;
    //}

    //public void ActivateTeleportPoint()
    //{
    //    capsuleCollider.enabled = true;
    //}
}
