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
        navMeshAgent.SetDestination(initialWaypoint.position);
    }

    public void SetFinalPlayerLocation()
    {
        navMeshAgent.SetDestination(finalWaypoint.position);
    }
}
