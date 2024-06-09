using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TherapistMarker : NetworkBehaviour
{
    public GameObject objectPrefab; // Assign this in the inspector

    private GameObject spawnedObject;

    void Update()
    {
        if (!IsOwner) return;

        // On left mouse button down, spawn the object
        if (Input.GetMouseButtonDown(0))
        {
            SpawnObject();
        }

        // On left mouse button hold, drag the object
        if (Input.GetMouseButton(0) && spawnedObject != null)
        {
            DragObject();
        }

        // On left mouse button up, destroy the object
        if (Input.GetMouseButtonUp(0) && spawnedObject != null)
        {
            DestroyObject();
        }
    }

    void SpawnObject()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectPrefab, GetMouseWorldPosition(), Quaternion.identity);
            var networkObject = spawnedObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn(true);
            }
        }
    }

    void DragObject()
    {
        if (spawnedObject != null)
        {
            spawnedObject.transform.position = GetMouseWorldPosition();
        }
    }

    void DestroyObject()
    {
        if (spawnedObject != null)
        {
            var networkObject = spawnedObject.GetComponent<NetworkObject>();
            if (networkObject != null && networkObject.IsOwner)
            {
                networkObject.Despawn(true);
                Destroy(spawnedObject);
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
