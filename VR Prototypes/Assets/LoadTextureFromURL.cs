using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadTextureFromURL : PuzzleListReader
{
    public string textureURL = "";
    public Material textureMaterial;
    public RawImage canvasImage;

    [SerializeField]
    private IntSO puzzleLevelSO;

    private void Awake()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "ImageLinks.txt");
        string jsonText = File.ReadAllText(jsonFilePath);
        myPuzzleList = JsonUtility.FromJson<PuzzleList>(jsonText);
        SanitizeURLs(myPuzzleList);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadImage(myPuzzleList.puzzlelevel[puzzleLevelSO.Value].textureURL));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DownloadImage(textureURL));
        }

    }

    private void SanitizeURLs(PuzzleList puzzleList)
    {
        foreach (var puzzleLevel in puzzleList.puzzlelevel)
        {
            puzzleLevel.textureURL = SanitizeURL(puzzleLevel.textureURL);
        }
    }

    private string SanitizeURL(string url)
    {
        // Remove any non-ASCII characters
        string sanitizedURL = new string(url.Where(c => c <= sbyte.MaxValue).ToArray());
        return sanitizedURL.Trim();
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