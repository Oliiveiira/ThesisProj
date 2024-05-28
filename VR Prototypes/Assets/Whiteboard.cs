using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Texture2D initialTexture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    public RawImage tvTexture;

    // Start is called before the first frame update
    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        initialTexture = new Texture2D((int)textureSize.x, (int)textureSize.y); 
        r.material.mainTexture = texture;
        tvTexture.texture = texture;
    }

    public void EraseDraw()
    {
        var r = GetComponent<Renderer>();
        texture = initialTexture;
        initialTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
        tvTexture.texture = texture;
    }
}
