using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TextureSender : NetworkBehaviour
{
    public RenderTexture textureToSend;
    public float sendInterval = 0.5f; // Send data every 0.5 seconds
    private float timeSinceLastSend = 0f;
    private byte[] previousData;
    private bool isSending = false;
    public ulong OwnerclientID;
    private IKTargetFollowVRRig player;

    // Max size of each chunk in bytes
    private const int MaxChunkSize = 1024;

    // Start is called before the first frame update
    void Start()
    {
        OwnerclientID = 0;
        player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<IKTargetFollowVRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer && isSending)
        {
            timeSinceLastSend += Time.deltaTime;
            if (timeSinceLastSend >= sendInterval)
            {
                SendTexture();
                timeSinceLastSend = 0f;
            }
        }
    }

    public void StartSending()
    {
        isSending = true;
    }

    public void StopSending()
    {
        isSending = false;
    }

    public void SendTexture()
    {
        Texture2D tex = new Texture2D(textureToSend.width, textureToSend.height, TextureFormat.RGBA32, false);
        RenderTexture.active = textureToSend;
        tex.ReadPixels(new Rect(0, 0, textureToSend.width, textureToSend.height), 0, 0);
        tex.Apply();

        byte[] currentData = tex.EncodeToJPG(25);
        Destroy(tex);

        if (previousData == null || !AreArraysEqual(currentData, previousData))
        {
            previousData = currentData;
            SendTextureDataInChunks(currentData);
        }
    }

    private void SendTextureDataInChunks(byte[] data)
    {
        int dataLength = data.Length;
        int numChunks = Mathf.CeilToInt(dataLength / (float)MaxChunkSize);

        // Send header with total data size
       // ReceiveTextureDataHeaderClientRpc(dataLength);
       if(player.currentPaintPosition == 0)
       {
            ReceiveTextureDataHeaderClientRpc(dataLength, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = GetAllClientIdsExcept((ulong)player.playerID)
                }
            });
           // ReceiveServerTextureDataHeaderServerRpc(dataLength);
       }
        else
        {
            ReceiveTextureDataHeaderClientRpc(dataLength, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = GetAllClientIdsExcept((ulong)player.playerID)
                }
            });
           // ReceiveServerTextureDataHeaderServerRpc(dataLength);
        }
       

        for (int i = 0; i < numChunks; i++)
        {
            int offset = i * MaxChunkSize;
            int chunkSize = Mathf.Min(MaxChunkSize, dataLength - offset);
            byte[] chunk = new byte[chunkSize];
            System.Array.Copy(data, offset, chunk, 0, chunkSize);
            //SendTextureDataChunkServerRpc(chunk);
            //ReceiveTextureDataChunkClientRpc(chunk);
            if (player.currentPaintPosition == 0)
            {
                ReceiveTextureDataChunkClientRpc(chunk, new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = GetAllClientIdsExcept((ulong)player.playerID)
                    }
                });
            }
            else
            {
                ReceiveTextureDataChunkClientRpc(chunk, new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = GetAllClientIdsExcept((ulong)player.playerID)
                    }
                });
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendTextureDataHeaderServerRpc(int totalSize)
    {
        ReceiveTextureDataHeaderClientRpc(totalSize);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendTextureDataChunkServerRpc(byte[] chunk)
    {
        ReceiveTextureDataChunkClientRpc(chunk);
    }

    [ClientRpc]
    private void ReceiveTextureDataHeaderClientRpc(int totalSize, ClientRpcParams clientRpcParams = default)
    {
        var textureReceiver = FindObjectOfType<TextureReceiver>();
        textureReceiver.ReceiveTextureDataHeader(totalSize);
    }

    [ClientRpc]
    private void ReceiveTextureDataChunkClientRpc(byte[] chunk, ClientRpcParams clientRpcParams = default)
    {
        var textureReceiver = FindObjectOfType<TextureReceiver>();
        textureReceiver.ReceiveTextureDataChunk(chunk);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReceiveServerTextureDataHeaderServerRpc(int totalSize)
    {
        var textureReceiver = FindObjectOfType<ServerTextureReceiver>();
        textureReceiver.ReceiveTextureDataHeader(totalSize);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReceiveServerTextureDataChunkServerRpc(byte[] chunk)
    {
        var textureReceiver = FindObjectOfType<ServerTextureReceiver>();
        textureReceiver.ReceiveTextureDataChunk(chunk);
    }

    private bool AreArraysEqual(byte[] a1, byte[] a2)
    {
        if (a1.Length != a2.Length)
            return false;

        for (int i = 0; i < a1.Length; i++)
        {
            if (a1[i] != a2[i])
                return false;
        }
        return true;
    }

    private ulong[] GetAllClientIdsExcept(ulong clientId)
    {
        List<ulong> clientIds = new List<ulong>();
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId != clientId)
            {
                clientIds.Add(client.ClientId);
            }
        }
        Debug.Log(clientIds.ToArray());
        return clientIds.ToArray();
    }
}
