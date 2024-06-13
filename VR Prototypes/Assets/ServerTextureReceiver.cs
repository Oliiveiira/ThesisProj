using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTextureReceiver : MonoBehaviour
{
    public Renderer imageTex; // Ensure this is assigned in the Inspector
    private List<byte> receivedData = new List<byte>();
    private int totalDataSize = -1;

    public void ReceiveTextureDataHeader(int totalSize)
    {
        totalDataSize = totalSize;
        receivedData.Clear();
    }

    public void ReceiveTextureDataChunk(byte[] chunk)
    {
        receivedData.AddRange(chunk);

        // Check if the data reception is complete
        if (IsDataComplete())
        {
            byte[] fullData = receivedData.ToArray();
            receivedData.Clear(); // Clear the buffer for the next reception
            ApplyTexture(fullData);
        }
    }

    private bool IsDataComplete()
    {
        return totalDataSize > 0 && receivedData.Count >= totalDataSize;
    }

    private void ApplyTexture(byte[] data)
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(data); // Load the received image data into the texture
        // Apply the texture to your target object
        // For example, you can set it to a material of a renderer
        imageTex.material.mainTexture = tex;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = tex;
        }
    }
}
