using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    [SerializeField]
    private Transform initialWaypoint;
    [SerializeField]
    private Transform finalWaypoint;

    [SerializeField]
    private GameObject canvas;

    //[SerializeField]
    //private GameEvent movePlayer;

    [SerializeField]
    public GameObject supermarketCart;

    private void Update()
    {
        if (navMeshAgent.velocity.magnitude > 0.01f)
        {
            supermarketCart.SetActive(false);
        }
        else
        {
            supermarketCart.SetActive(true);
        }
    }

    public void SetinitialPlayerLocation()
    {
        canvas.SetActive(false);
        navMeshAgent.SetDestination(initialWaypoint.position);
    }

    public void SetFinalPlayerLocation()
    {
        navMeshAgent.SetDestination(finalWaypoint.position);
    }

    //public void MovePlayer()
    //{
    //    canvas.SetActive(false);
    //    movePlayer.Raise();
    //}
}
