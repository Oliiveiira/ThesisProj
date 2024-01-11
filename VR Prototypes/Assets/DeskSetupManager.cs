using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeskSetupManager : PuzzleDeskPivotReader
{
    public GameObject visual;
    public Transform pivot;
    public Transform creationHand;

    public float heightOffset;
    public float defaultWidht = 0.3f;
    public float defaultHeight = 0.01f;

    public GameObject ObjectToSpawn;

    public OVRPassthroughLayer updateShapePassthrough;

    private Vector3 startPosition;
    private bool isUpdatingShape;

    [SerializeField]
    private FloatSO deskPivotX;
    [SerializeField]
    private FloatSO deskPivotY;

    [SerializeField]
    private TextMeshPro instruction;

    [SerializeField]
    private PuzzleDeskPivotWritter pivotWritter;

    // Start is called before the first frame update
    void Start()
    {
        visual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            visual.SetActive(true);
            startPosition = creationHand.position;
            isUpdatingShape = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            isUpdatingShape = false;
            ObjectToSpawn.SetActive(true);
            updateShapePassthrough.hidden = true;
            pivotWritter.myDeskPivot.deskPivotX = pivot.position.x;
            pivotWritter.myDeskPivot.deskPivotY = pivot.position.y;
            pivotWritter.SavePivotToJson();
            //deskPivotX.Value = pivot.position.x;
            //deskPivotY.Value = pivot.position.y;
            instruction.gameObject.SetActive(false);
        }

        if (isUpdatingShape)
        {
            UpdateShape();
        }
    }

    public void UpdateShape()
    {
        //scale the cube
        float distance = Vector3.ProjectOnPlane(creationHand.position - startPosition, Vector3.up).magnitude;
        visual.transform.localScale = new Vector3(distance, defaultHeight, defaultWidht);

        //Rotation
        //pivot.right = Vector3.ProjectOnPlane(creationHand.position - startPosition, Vector3.up);

        //Position
       // pivot.position = startPosition + pivot.rotation * new Vector3(visual.transform.localScale.x / 2, heightOffset, visual.transform.localScale.z / 2);
       // pivot.position = startPosition + new Vector3(visual.transform.localScale.x / 2, heightOffset, visual.transform.localScale.z / 2);
        pivot.position = new Vector3(0.2f, startPosition.y + heightOffset, 0);
    }
}
