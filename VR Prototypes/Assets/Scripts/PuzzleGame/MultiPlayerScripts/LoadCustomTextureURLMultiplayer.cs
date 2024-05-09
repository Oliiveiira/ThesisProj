using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadCustomTextureURLMultiplayer : CustomLevelsData
{
    public string textureURL = "";
    public Material textureMaterial;
    public RawImage canvasImage;

    //[SerializeField]
    //private IntSO puzzleLevelSO;

    //private void Awake()
    //{
    //    string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
    //    string jsonText = File.ReadAllText(jsonFilePath);
    //    myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
    //}
    // Start is called before the first frame update
    void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "CustomLevelsData.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myLevelData = JsonUtility.FromJson<MultiplayerLevelsData>(jsonText);
        StartCoroutine(DownloadImage(myLevelData.levelData[0].textureURL));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DownloadImage(textureURL));
        }

    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        Debug.Log(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
            //this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            textureMaterial.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        canvasImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}
