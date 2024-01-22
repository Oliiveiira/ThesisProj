using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;

public class SetPuzzlePieceOwner : HandGrabInteractor
{
    public override void Select()
    {
        var interactable = Interactable.GetComponent<PuzzlePieceMultiplayer>();
        Debug.Log("Selected");
        interactable.SetClientOwnershipServerRPC();

        //if (interactable.ChangeOwnership())
        //{
        //    Debug.Log("HoverObject");
        //}
    }

    //protected override void InteractableUnselected(HandGrabInteractable interactable)
    //{
    //    var virtualObject = interactable.GetComponent<PuzzlePieceMultiplayer>();
    //    virtualObject.RemoveOwnershipServerRPC();
    //    Debug.Log("Released");
    //    base.InteractableUnselected(interactable);
    //}
}
