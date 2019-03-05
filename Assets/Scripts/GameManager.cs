using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cameraPivot;
    public GameObject player;
    public GameObject[] uncheckedPlatforms;    // [0] = rotation 0; [1] = rotation -90(90); [2] = rotation -180(180); [3] = rotation 90(270)
    public List<GameObject> frontPlatforms;
    public List<GameObject> rightPlatforms;
    public List<GameObject> backPlatforms;
    public List<GameObject> leftPlatforms;
    public float yStart;
    public float yEnd;
    public int currentRotationStep;
    private List<char> rotateQue = new List<char>();
    private int rotateCount;
    private Player_v2 playerScript;
    private int desiredRotation;    // where we should be rotating to 
    public int highestX;
    public int lowestX;
    public int highestZ;
    public int lowestZ;
    public int lowestY;
    public int highestY;
    private bool rotateNeeded; // not used - how many rotations need to be done
    static private float yTime;
    static private float xTime;

    public float xStart;
    public float xEnd;
    public float OGxStart;
    public float OGxEnd;
    public bool swapped;

    // Use this for initialization
    void Start()
    {
        playerScript = player.GetComponent<Player_v2>();
        currentRotationStep = 0;
        uncheckedPlatforms = GameObject.FindGameObjectsWithTag("platform");
        Vector3 firstObject = uncheckedPlatforms[0].GetComponent<Transform>().position;
        // set all highest and lowest points to first box
        highestX = (int)firstObject.x;
        lowestX = (int)firstObject.x;
        highestZ = (int)firstObject.z;
        lowestZ = (int)firstObject.z;
        lowestY = (int)firstObject.y;
        highestY = (int)firstObject.y;
        yStart = 0.0f;
        yEnd = 0.0f;
        rotateNeeded = false;
        yTime = 0.0f;
        xTime = 0.0f;
        xStart = 0.0f;
        xEnd = 15.0f;
        OGxStart = 0.0f;
        OGxEnd = 15.0f;

        // go through each tile to check if it is the highest in the X and Z or the lowest
        for (int i = 1; i < uncheckedPlatforms.Length; i++)
        {
            // transform.TransformPoint()
            Vector3 currentLoc = uncheckedPlatforms[i].transform.position;
            if (currentLoc.x > highestX)
            {
                highestX = (int)uncheckedPlatforms[i].transform.position.x;
            }
            else if (currentLoc.x < lowestX)
            {
                lowestX = (int)uncheckedPlatforms[i].transform.position.x;
            }
            if (currentLoc.z > highestZ)
            {
                highestZ = (int)uncheckedPlatforms[i].transform.position.z;
            }
            else if (currentLoc.z < lowestZ)
            {
                lowestZ = (int)uncheckedPlatforms[i].transform.position.z;
            }
            if (currentLoc.y > highestY)
            {
                highestY = (int)uncheckedPlatforms[i].transform.position.y;
            }
            else if (currentLoc.y < lowestY)
            {
                lowestY = (int)uncheckedPlatforms[i].transform.position.y;
            }
        }
        float currentY = lowestY;
        // go through the four different views and assign which platforms on which side view
        for (int angle = 0; angle < 4; angle++)
        {
            RaycastHit hit;
            Vector3 origin;
            Vector3 direction;
            switch (angle)
            {
                // front
                case 0:
                    // checks for which tiles are in the front
                    for (int x = lowestX; x <= highestX; x++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(x, y, lowestZ - 1);
                            direction = Vector3.forward;

                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                if (hit.transform.gameObject.layer == 9)
                                {
                                    frontPlatforms.Add(hit.collider.gameObject);
                                    // not sure what this did, but doesn't do anything useful so far so eh
                                    /*
                                    if (frontPlatforms[0].transform.position.z < hit.collider.gameObject.transform.position.z)
                                    {
                                        //frontPlatforms.Insert(0, hit.collider.gameObject);
                                        //frontPlatforms.RemoveAt(frontPlatforms.Count - 1);
                                    }*/
                                }
                            }
                        }
                    }
                    break;
                // right
                case 1:
                    for (int z = lowestZ; z <= highestZ; z++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(highestX + 1, y, z);
                            direction = Vector3.left;

                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                if (hit.transform.gameObject.layer == 9)
                                {
                                    rightPlatforms.Add(hit.collider.gameObject);
                                    // not sure what this did, but doesn't do anything useful so far so eh
                                    /*
                                    if (rightPlatforms[0].transform.position.x < hit.collider.gameObject.transform.position.x)
                                    {
                                        //rightPlatforms.Insert(0, hit.collider.gameObject);
                                        //rightPlatforms.RemoveAt(rightPlatforms.Count - 1);
                                    }*/
                                }
                            }
                        }
                    }
                    break;
                // back
                case 2:
                    for (int x = lowestX; x <= highestX; x++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(x, y, highestZ + 1);
                            direction = Vector3.back;
                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                if (hit.transform.gameObject.layer == 9)
                                {
                                    backPlatforms.Add(hit.collider.gameObject);
                                    // not sure what this did, but doesn't do anything useful so far so eh
                                    /*
                                    if (backPlatforms[0].transform.position.z > hit.collider.gameObject.transform.position.z)
                                    {
                                        //backPlatforms.Insert(0, hit.collider.gameObject);
                                        //backPlatforms.RemoveAt(backPlatforms.Count - 1);
                                    } */
                                }
                            }
                        }
                    }
                    break;
                // left
                case 3:
                    for (int z = lowestZ; z <= highestZ; z++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(lowestX - 1, y, z);
                            direction = Vector3.right;

                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                if (hit.transform.gameObject.layer == 9)
                                {
                                    leftPlatforms.Add(hit.collider.gameObject);
                                    // not sure what this did, but doesn't do anything useful so far so eh
                                    /*if (leftPlatforms[0].transform.position.x > hit.collider.gameObject.transform.position.x)
                                    {
                                        leftPlatforms.Insert(0, hit.collider.gameObject);
                                        leftPlatforms.RemoveAt(leftPlatforms.Count - 1);
                                    }*/
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /*grab the front platforms and move them to the front
        foreach (var platform in frontPlatforms)
        {
            float zPos = lowestZ - platform.transform.position.z;
            Vector3 platformPos = platform.GetComponent<BoxCollider>().center;
            platformPos = new Vector3(0, 0, zPos);
            platform.GetComponent<BoxCollider>().center = platformPos;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardCheck();
        StartRotation();
        if (rotateNeeded)
            RotateScene();
    }

    void KeyboardCheck()
    {
        // rotations are done one at a time
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && rotateCount < 2) 
        {
            // add to the que of rotations
            if(Input.GetKeyDown(KeyCode.Q)) { rotateQue.Add('Q'); }
            else { rotateQue.Add('E'); }
            playerScript.notRotating = false;
            rotateCount++;
        }
    }

    /// <summary>
    /// If there are rotations still in the que and the camera is not rotating, perform a rotation
    /// </summary>
    void StartRotation()
    {
        /// QUE BUFFERS
        /// every .33 seconds check the top of the que and perform the next entry (if there is)
        /// every frame check if an input is a second old, and remove any that are more than a second old.
        /// make a global time variable, add fixed delta time to it
        if (rotateQue.Count > 0)
        {
            char nextInput = rotateQue[0];
            rotateQue.RemoveAt(0);
            yStart = Mathf.Lerp(yStart, yEnd, yTime) % 360;

            // Rotate the camera to the left when Q is pressed
            if (nextInput == 'Q')
            {
                currentRotationStep++;
            }
            // Rotate the camera to the right when E is pressed
            else
            {
                currentRotationStep--;
            }

            // changes where it is that pivot should rotate to
            yEnd = currentRotationStep * 90;
            if(rotateNeeded == true)
            {
                xStart = Mathf.Lerp(xStart, xEnd, xTime);
                xEnd = 15.0f;
                swapped = false;
            }

            xTime = 0.0f;
            // set xStart to the current pos for xLerp
            yTime = 0;
            rotateNeeded = true;
        }
    }
    void RotateScene()
    {
        yTime += 1.5f * (Time.deltaTime);
        xTime += 3.0f * (Time.deltaTime);

        Vector3 pos = new Vector3(/*15 * Mathf.Sin(yTime * Mathf.PI)*/Mathf.Lerp(xStart, xEnd, xTime), Mathf.Lerp(yStart, yEnd, yTime), 0);
        cameraPivot.transform.rotation = Quaternion.Euler(pos.x, pos.y, 0);
        player.transform.rotation = Quaternion.Euler(pos.x, pos.y, 0);

        // if xtime is at .5, swap the start and end
        // if ytime
        if(yTime >= 0.5f && !swapped)
        {
            swapped = true;
            xStart = OGxEnd;
            xEnd = OGxStart;
            xTime = 0.0f;
        }

        if (yTime >= 1.0f)
        {
            currentRotationStep %= 4;
            yTime = 1;
            xTime = 1;
            rotateNeeded = false;
            swapped = false;
            
            playerScript.notRotating = true;

            if (rotateQue.Count == 0)
            {
                Vector3 newPlayerPos = new Vector3();
                newPlayerPos = player.transform.position;
                // changes what direction is to the right
                switch (currentRotationStep % 4)
                {
                    case 0:
                        Debug.Log("Case 0");
                        playerScript.side = 0;
                        playerScript.forward = new Vector3(1, 0, 0);
                        break;
                    case -1:
                    case 3:
                        Debug.Log("Case -1 or 3");
                        playerScript.side = 1;
                        playerScript.forward = new Vector3(0, 0, 1);
                        break;
                    case 2:
                    case -2:
                        Debug.Log("Case -2 or 2");
                        playerScript.side = 2;
                        playerScript.forward = new Vector3(-1, 0, 0);                        
                        break;
                    case 1:
                    case -3:
                        Debug.Log("Case 1 or -3");
                        playerScript.side = 3;
                        playerScript.forward = new Vector3(0, 0, -1);
                        break;
                    default:
                        Debug.Log("Default because: " + currentRotationStep % 4);
                        break;
                }

                pos = new Vector3(/*15 * Mathf.Sin(yTime * Mathf.PI)*/Mathf.Lerp(xStart, xEnd, xTime), Mathf.Lerp(yStart, yEnd, yTime), 0);
                cameraPivot.transform.rotation = Quaternion.Euler(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
                player.transform.rotation = Quaternion.Euler(pos.x, pos.y, 0);

                xStart = OGxStart;
                xEnd = OGxEnd;

                rotateCount = 0;
            }
            
        }


    }
}


