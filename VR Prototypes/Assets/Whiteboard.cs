using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Whiteboard : NetworkBehaviour
{
    public Texture2D texture;
    public Texture2D initialTexture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    public RawImage tvTexture;

    private List<Vector2Int> pixelChanges = new List<Vector2Int>();
    private List<Color[]> colorChanges = new List<Color[]>();

    // Start is called before the first frame update
    void Start()
    {
        //if (!IsServer)
        //    return;
        //SendCanvasTextureClientRPC();
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        initialTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
        tvTexture.texture = texture;
    }

    [ServerRpc]
    public void UpdateTextureServerRpc(int x, int y, Color[] colors, int penSize, ulong clientId)
    {
        texture.SetPixels(x, y, penSize, penSize, colors);
        texture.Apply();
        UpdateTextureClientRpc(x, y, colors, penSize, clientId);
    }

    [ClientRpc]
    private void UpdateTextureClientRpc(int x, int y, Color[] colors, int penSize, ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId != clientId)
        {
            texture.SetPixels(x, y, penSize, penSize, colors);
            texture.Apply();
        }
    }
    
    //[ClientRpc]
    //public void SendCanvasTextureClientRPC()
    //{
    //    var r = GetComponent<Renderer>();
    //    texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
    //    initialTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
    //    r.material.mainTexture = texture;
    //    tvTexture.texture = texture;
    //}

    public void EraseDraw()
    {
        var r = GetComponent<Renderer>();
        texture = initialTexture;
        initialTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
        tvTexture.texture = texture;
    }
}
