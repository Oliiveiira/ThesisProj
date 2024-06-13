using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Texture texture;
    public Camera renderCamera;
    //public RawImage tvTexture;

    // Start is called before the first frame update
    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = r.material.mainTexture;
        renderCamera = GetComponentInChildren<Camera>();
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = Color.white;

       // tvTexture.texture = texture;
        StartCoroutine(DelayCanvasChange(2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayCanvasChange(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Send a request to the server to remove ownership
        renderCamera.clearFlags = CameraClearFlags.Depth;
    }
}
