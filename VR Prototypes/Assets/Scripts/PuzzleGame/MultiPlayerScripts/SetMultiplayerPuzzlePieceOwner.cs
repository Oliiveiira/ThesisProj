using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;

public class SetMultiplayerPuzzlePieceOwner : HandGrabInteractor
{
    private bool isSelected;

    private Puzzle3DPieceMultiPlayer interactable;

    public override void Select()
    {
        interactable = Interactable.GetComponent<Puzzle3DPieceMultiPlayer>();
        Debug.Log("Selected");
        if (!isSelected)
        {
            interactable.SetOutlineMaterial();
            isSelected = true;
        }

        interactable.SetClientOwnershipServerRPC();

        //if (interactable.ChangeOwnership())
        //{
        //    Debug.Log("HoverObject");
        //}
    }

    //public override void Unselect()
    //{
    //    var interactable = Interactable.GetComponent<PuzzlePieceMultiplayer>();
    //    interactable.DisableOutlineMaterial();
    //    Debug.Log("Unselected");
    //    //isSelected = false;
    //    base.Unselect();
    //    //interactable.DisableOutlineMaterial();
    //    Debug.Log("Unselected");
    //}

    protected override void Update()
    {
        //Debug.Log(State);
        base.Update();
        if(State != InteractorState.Hover && isSelected)
        {
            //var interactable = Interactable.GetComponent<PuzzlePieceMultiplayer>();
            interactable.DisableOutlineMaterial();
            Debug.Log(interactable.gameObject.name);
            Debug.Log("Unselected");
            isSelected = false;
        }
    }

    //protected override void InteractableUnselected(HandGrabInteractable interactable)
    //{
    //    interactable.GetComponent<PuzzlePieceMultiplayer>();
    //    virtualObject.RemoveOwnershipServerRPC();
    //    Debug.Log("Released");
    //    base.InteractableUnselected(interactable);
    //    Debug.Log("Released");
    //}
}
