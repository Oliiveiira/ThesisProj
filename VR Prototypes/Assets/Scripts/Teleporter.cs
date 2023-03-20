using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

public class Teleporter : MonoBehaviour
{
    public float velocity = 10f;
    public float angle = 45f;
    public float gravity = 9.8f;
    public float maxDistance = 50f;

    public int raylenght = 10;
    public float delay = 0.1f;
    public Vector3 teleportPos;

    public GameObject pointer;
    public ActiveStateSelector laserPose;
    public ActiveStateSelector tpPose;
    public LineRenderer lr;

    public GameObject playerPosition;
    public bool showLaser;
    public bool canTP;

    public float cooldown;
    float lastTp;

    public GameObject ovrCameraRig;

    private Vector3[] positions;

    [SerializeField]
    private GameEvent ActivateTpPoint;

    void Start()
    {
        laserPose.WhenSelected += () => showLaser = true;
        laserPose.WhenUnselected += () => showLaser = false;

        tpPose.WhenSelected += () => canTP = true;
        tpPose.WhenUnselected += () => canTP = false;
    }

    void Update()
    {
        TeleporttoPoint();
    }

    private void TeleporttoPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raylenght * 10))
        {
            teleportPos = hit.point;

            if (showLaser)
            {
                pointer.SetActive(true);
                lr.enabled = true;
                // set up the line renderer positions
                positions = CalculateParabolicLine(transform.position, teleportPos, 0, Mathf.RoundToInt(maxDistance / delay) + 1);
                lr.positionCount = positions.Length;
                lr.SetPositions(positions);
                pointer.transform.position = new Vector3(teleportPos.x, 0, teleportPos.z);

                if (canTP && Time.time - lastTp > cooldown && hit.collider.gameObject.tag == "TeleportDestination")
                {
                    ovrCameraRig.SetActive(false);
                    playerPosition.transform.position = new Vector3(hit.point.x, 1.414f, hit.point.z);
                    ovrCameraRig.SetActive(true);
                    ActivateTpPoint.Raise();
                    lastTp = Time.time;
                }
            }
            else
            {
                lr.enabled = false;
                pointer.SetActive(false);
            }
        }
    }

    private Vector3[] CalculateParabolicLine(Vector3 start, Vector3 end, float height, int numPoints)
    {
        Vector3[] positions = new Vector3[numPoints];

        // Calculate the distance between the start and end points
        float distance = Vector3.Distance(start, end);

        // Calculate the time it would take for an object to travel the distance at the given velocity
        float totalTime = distance / velocity;

        // Calculate the x and z components of the velocity vector
        float vx = (end.x - start.x) / totalTime;
        float vz = (end.z - start.z) / totalTime;

        // Calculate the initial y velocity component to reach the given height
        float vy = (height - start.y + end.y) / totalTime + 0.5f * gravity * totalTime;

        // Calculate the time interval between points
        float timeInterval = totalTime / (numPoints - 1);

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * timeInterval;

            // Calculate the x and z coordinates at time t
            float x = start.x + vx * t;
            float z = start.z + vz * t;

            // Calculate the y coordinate at time t using the parabolic formula
            float y = start.y + vy * t - 0.5f * gravity * t * t;

            // If the calculated y coordinate is less than 0, set it to 0 to ensure the end point is at ground level
            if (y < 0)
            {
                y = 0;
            }

            // Set the position of the point at index i
            positions[i] = new Vector3(x, y, z);
        }

        return positions;
    }

    //private Vector3[] CalculateLineRendererPositions(Vector3 start, Vector3 end)
    //{
    //    int numPositions = Mathf.RoundToInt(maxDistance / delay) + 1;
    //    Vector3[] positions = new Vector3[numPositions];
    //    float totalTime = maxDistance / velocity;
    //    timeInterval = totalTime / numPositions;

    //    float x = start.x;
    //    float y = start.y;
    //    float z = start.z;
    //    float vx = velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
    //    float vy = velocity * Mathf.Sin(angle * Mathf.Deg2Rad);

    //    for (int i = 0; i < numPositions; i++)
    //    {
    //        float t = i * timeInterval;
    //        x = start.x + vx * t;
    //        y = start.y + vy * t - 0.5f * gravity * t * t;
    //        z = start.z + (end - start).normalized.magnitude * velocity * Mathf.Cos(angle * Mathf.Deg2Rad) * t;

    //        positions[i] = new Vector3(x, y, z);
    //    }

    //    positions[0] = start;
    //    positions[numPositions - 1] = end;

    //    return positions;
    //}
    //// calculate the positions of the line renderer points for the parabolic line
    //private Vector3[] CalculateLineRendererPositions(Vector3 start, Vector3 end)
    //{
    //    int numPositions = Mathf.RoundToInt(maxDistance / delay) + 1;
    //    Vector3[] positions = new Vector3[numPositions];
    //    timeInterval = maxDistance / velocity / numPositions;

    //    float x = 0f;
    //    float y = 0f;
    //    float z = 0f;
    //    for (int i = 0; i < numPositions; i++)
    //    {
    //        x = start.x + (velocity * x * timeInterval);
    //        y = start.y + (velocity * y * timeInterval) - (0.5f * gravity * Mathf.Pow(timeInterval * i, 2));
    //        z = start.z + (velocity * z * timeInterval);

    //        positions[i] = new Vector3(x, y, z);
    //    }

    //    positions[0] = start;
    //    positions[numPositions - 1] = end;

    //    return positions;
    //}
}


//public class Teleporter : MonoBehaviour
//{
//    //public Transform startTransform;
//    public float velocity = 10f;
//    public float angle = 45f;
//    public float gravity = 9.8f;
//    public float maxDistance = 50f;


//    private float timeInterval;
//    private Vector3[] positions;

//    public int raylenght = 10;
//    public float delay = 0.1f;
//    Vector3 teleportPos = new Vector3();

//    public Material tmat;
//    public GameObject pointer;
//    public ActiveStateSelector laserPose;
//    public ActiveStateSelector tpPose;
//    public LineRenderer lr;

//    public GameObject playerPosition;
//    public bool showLaser;
//    public bool canTP;

//    public float cooldown;
//    float lastTp;

//    public OVRPlayerController playerController;
//    public CharacterController cc;
//    // Start is called before the first frame update
//    void Start()
//    {
//        laserPose.WhenSelected += () => showLaser = true;
//        laserPose.WhenUnselected += () => showLaser = false;

//        tpPose.WhenSelected += () => canTP = true;
//        tpPose.WhenUnselected += () => canTP = false;

//        //positions = new Vector3[lr.positionCount];
//        //timeInterval = (maxDistance / velocity) / (lr.positionCount - 1);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        TeleporttoPoint();
//    }

//    private void TeleporttoPoint()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, transform.forward, out hit, raylenght * 10))
//        {
//            teleportPos = hit.point;

//            if (showLaser)
//            {
//                lr.enabled = true;
//                //lr.SetPosition(0, transform.position);
//                //lr.SetPosition(1, hit.point);
//                //pointer.transform.position = hit.point;

//                //for (int i = 0; i < lr.positionCount; i++)
//                //{
//                //    float t = i / (float)(lr.positionCount - 1);
//                //    positions[i] = CalculatePosition(t, hit.distance);
//                //}
//                lr.SetPosition(0, transform.position);
//                lr.SetPosition(1, hit.point);
//                //lr.SetPositions(positions);

//                if (canTP && Time.time-lastTp>cooldown && hit.collider.gameObject.tag == "TeleportDestination")
//                {
//                    playerController.transform.position = teleportPos;
//                    playerPosition.transform.position = teleportPos;
//                    cc.transform.position = teleportPos;
//                    lastTp = Time.time;
//                }
//            }
//            else
//            {
//                lr.enabled = false;
//               // lr.positionCount = 0;
//            }

//        }
//    }

//    private Vector3 CalculatePosition(float t, float hitDistance)
//    {
//        float x = Mathf.Lerp(0, hitDistance, t);
//        float y = x * Mathf.Tan(angle * Mathf.Deg2Rad) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Cos(angle * Mathf.Deg2Rad)));
//        return transform.position + transform.forward * x + transform.up * y;
//    }
//}
//using UnityEngine;

//public class ParabolicLineRenderer : MonoBehaviour
//{
//    public LineRenderer lineRenderer;
//    public Transform startTransform;
//    public float velocity = 10f;
//    public float angle = 45f;
//    public float gravity = 9.8f;
//    public float maxDistance = 50f;

//    private float timeInterval;
//    private Vector3[] positions;

//    private void Start()
//    {
//        positions = new Vector3[lineRenderer.positionCount];
//        timeInterval = (maxDistance / velocity) / (lineRenderer.positionCount - 1);
//    }

//    private void Update()
//    {
//        Ray ray = new Ray(startTransform.position, startTransform.forward);
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit, maxDistance))
//        {
//            for (int i = 0; i < lineRenderer.positionCount; i++)
//            {
//                float t = i / (float)(lineRenderer.positionCount - 1);
//                positions[i] = CalculatePosition(t, hit.distance);
//            }

//            lineRenderer.SetPositions(positions);
//        }
//        else
//        {
//            lineRenderer.positionCount = 0;
//        }
//    }

//    private Vector3 CalculatePosition(float t, float hitDistance)
//    {
//        float x = Mathf.Lerp(0, hitDistance, t);
//        float y = x * Mathf.Tan(angle * Mathf.Deg2Rad) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Cos(angle * Mathf.Deg2Rad)));
//        return startTransform.position + startTransform.forward * x + startTransform.up * y;
//    }
//}
