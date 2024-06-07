using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Pen : NetworkBehaviour
{
    [SerializeField]
    private Transform tip;
    [SerializeField]
    private int penSize = 5;

    private Renderer tipRenderer;
    private Color[] colors;
    private float tipHeight;
    private Whiteboard whiteBoard;
    private Vector2 hitPos;
    private Vector2 lastTouchPos;
    private bool touchlastFrame;
    private Quaternion lastTouchRot;
    public Transform ray;
    public int layer_mask;

    // Start is called before the first frame update
    void Start()
    {
        tipRenderer = tip.GetComponent<Renderer>();
        colors = Enumerable.Repeat(tipRenderer.material.color, penSize * penSize).ToArray();
        tipHeight = tip.lossyScale.z;
        layer_mask = LayerMask.GetMask("CanvasLayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tipRenderer.material.color = Color.blue;
            colors = Enumerable.Repeat(tipRenderer.material.color, penSize * penSize).ToArray();
        }
        Draw();
    }

    public void Draw()
    {
        if (Physics.Raycast(ray.position, transform.forward, out RaycastHit hit, tipHeight, layer_mask))
        {
            if (hit.transform.CompareTag("Canvas"))
            {
                if (whiteBoard == null)
                {
                    whiteBoard = hit.transform.GetComponent<Whiteboard>();
                }

                hitPos = new Vector2(hit.textureCoord.x, hit.textureCoord.y);

                var x = (int)(hitPos.x * whiteBoard.textureSize.x - (penSize / 2));
                var y = (int)(hitPos.y * whiteBoard.textureSize.y - (penSize / 2));

                if (y < 0 || y > whiteBoard.textureSize.y || x < 0 || x > whiteBoard.textureSize.x)
                    return;

                if (touchlastFrame)
                {
                    Debug.Log("touchedlastframe");
                    whiteBoard.texture.SetPixels(x, y, penSize, penSize, colors);
                    whiteBoard.UpdateTextureServerRpc(x, y, colors, penSize, NetworkManager.Singleton.LocalClientId);

                    for (float f = 0.01f; f < 1.00f; f += 0.03f)
                    {
                        var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);
                        whiteBoard.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                        whiteBoard.UpdateTextureServerRpc(lerpX, lerpY, colors, penSize, NetworkManager.Singleton.LocalClientId);
                    }

                   //transform.rotation = lastTouchRot;
                    Debug.Log(whiteBoard.texture);
                    whiteBoard.texture.Apply();
                }
                lastTouchPos = new Vector2(x, y);
                //lastTouchRot = transform.rotation;
                touchlastFrame = true;
                return;
            }
        }
        whiteBoard = null;
        touchlastFrame = false;
    }

    void OnDrawGizmos()
    {
        if (tip != null)
        {
            // Draw the raycast line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.position, ray.position + transform.forward * tipHeight);

            // Draw the hit point if available
            if (Physics.Raycast(ray.position, transform.forward, out RaycastHit hit, tipHeight))
            {
                if (hit.transform.CompareTag("Canvas"))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(hit.point, 0.01f);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pallete"))
        {
            Debug.Log("PickColor");
            Renderer palleteRenderer = other.GetComponent<Renderer>();
            tipRenderer.material.color = palleteRenderer.material.color;
            colors = Enumerable.Repeat(tipRenderer.material.color, penSize * penSize).ToArray();
        }
    }
}
