using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    public CharacterController cC;
    private ObjectGrabbable objectGrabbable;
    private Vector3 initialVelocity;
    // Start is called before the first frame update
    void Start()
    {
        initialVelocity = cC.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                float pickUpDistance = 5f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }

                }
            }
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
      
        }
    }

    public void FreezeCharacter()
    {
        cC.Move(Vector3.zero);
    }

    public void UnfreezeCharacter()
    {
        cC.Move(initialVelocity);
    }
}
