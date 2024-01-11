using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TablePivotManager : PuzzleDeskPivotReader
{
    [SerializeField]
    private FloatSO desktPositionX;
    [SerializeField]
    private FloatSO desktPositionY;

    // Start is called before the first frame update
    void Start()
    {
        // transform.position = new Vector3(desktPositionX.Value, desktPositionY.Value, 0);
        string jsonFileName = "PuzzleDeskPivot.txt";
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // Check if the file exists in the persistent data path
        string jsonText = File.ReadAllText(jsonFilePath);
        myDeskPivot = JsonUtility.FromJson<DeskPivot>(jsonText);

        transform.position = new Vector3(myDeskPivot.deskPivotX, myDeskPivot.deskPivotY, 0);
    }
}
