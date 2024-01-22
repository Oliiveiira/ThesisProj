using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;

public class SetPuzzlePieceOwner : HandGrabInteractor
{
    protected override void InteractableSelected(HandGrabInteractable interactable)
    {
        var virtualObject = interactable.GetComponent<PuzzlePieceMultiplayer>();
        virtualObject.SetClientOwnershipServerRPC();
        Debug.Log("ObjectSelected");
        //base.InteractableSelected(interactable);
        //    Debug.Log("ObjectSelected");
        //if (virtualObject.ChangeOwnership())
        //{
        //    base.InteractableSelected(interactable);
        //}
    }
    //protected override void DoHoverUpdate()
    //{
    //    //Debug.Log("HoverObject");
    //    PuzzlePieceMultiplayer puzzlePiece = GetComponent<PuzzlePieceMultiplayer>();
    //    puzzlePiece.SetClientOwnershipServerRPC();
    //    Debug.Log("HoverObject");
    //    base.DoHoverUpdate();

    //    //if (puzzlePiece.ChangeOwnership())
    //    //{
    //    //    Debug.Log("Client is now the owner");
    //    //    base.DoHoverUpdate();
    //    //}

    //}

    //protected override void InteractableUnselected(HandGrabInteractable interactable)
    //{
    //    var virtualObject = interactable.GetComponent<PuzzlePieceMultiplayer>();
    //    virtualObject.RemoveOwnershipServerRPC();
    //    Debug.Log("Released");
    //    base.InteractableUnselected(interactable);
    //}
}
