using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject cameraPivot;
    public GameObject player;
    public GameObject[] unCheckedlatforms;    // [0] = rotation 0; [1] = rotation -90(90); [2] = rotation -180(180); [3] = rotation 90(270)
    public List<GameObject> frontPlatforms;
    public List<GameObject> rightPlatforms;
    public List<GameObject> backPlatforms;
    public List<GameObject> leftPlatforms;
    public float yStart;
    public float yEnd;
    public int currentRotationStep;
    private List<char> rotateQue = new List<char>();
    private Player_v2 playerScript;
    private int desiredRotation;    // where we should be rotating to 
    private int highestX;
    private int lowestX;
    private int highestZ;
    private int lowestZ;
    private int lowestY;
    private int highestY;
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
        unCheckedlatforms = GameObject.FindGameObjectsWithTag("ground");
        Vector3 firstObject = transform.TransformPoint(unCheckedlatforms[0].GetComponent<Transform>().position);
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
        for (int i = 1; i < unCheckedlatforms.Length; i++)
        {

            Vector3 currentLoc = transform.TransformPoint(unCheckedlatforms[i].GetComponent<Transform>().position);
            if (currentLoc.x > highestX)
            {
                highestX = (int)unCheckedlatforms[i].GetComponent<Transform>().position.x;
            }
            else if (currentLoc.x < lowestX)
            {
                lowestX = (int)unCheckedlatforms[i].GetComponent<Transform>().position.x;
            }
            if (currentLoc.z > highestZ)
            {
                highestZ = (int)unCheckedlatforms[i].GetComponent<Transform>().position.z;
            }
            else if (currentLoc.z < lowestZ)
            {
                lowestZ = (int)unCheckedlatforms[i].GetComponent<Transform>().position.z;
            }
            if (currentLoc.y > highestY)
            {
                highestY = (int)unCheckedlatforms[i].GetComponent<Transform>().position.y;
            }
            else if (currentLoc.y < lowestY)
            {
                lowestY = (int)unCheckedlatforms[i].GetComponent<Transform>().position.y;
            }
        }
        float currentY = lowestY;
        // go through the four different views
        for (int angle = 0; angle < 4; angle++)
        {
            RaycastHit hit;
            Vector3 origin;
            Vector3 direction;
            switch (angle)
            {
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
                                frontPlatforms.Add(hit.collider.gameObject);
                                if(frontPlatforms[0].transform.position.z < hit.collider.gameObject.transform.position.z)
                                {
                                    frontPlatforms.Insert(0, hit.collider.gameObject);
                                    frontPlatforms.RemoveAt(frontPlatforms.Count-1);
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    for (int z = lowestZ; z <= highestZ; z++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(highestX + 1, y, z);
                            direction = Vector3.left;

                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                rightPlatforms.Add(hit.collider.gameObject);
                                if (rightPlatforms[0].transform.position.x < hit.collider.gameObject.transform.position.x)
                                {
                                    rightPlatforms.Insert(0, hit.collider.gameObject);
                                    rightPlatforms.RemoveAt(rightPlatforms.Count-1);
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    // 
                    for (int x = lowestX; x <= highestX; x++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(x, y, highestZ + 1);
                            direction = Vector3.back;

                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                backPlatforms.Add(hit.collider.gameObject);
                                if (backPlatforms[0].transform.position.z > hit.collider.gameObject.transform.position.z)
                                {
                                    backPlatforms.Insert(0, hit.collider.gameObject);
                                    backPlatforms.RemoveAt(backPlatforms.Count-1);
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    // 
                    for (int z = lowestZ; z <= highestZ; z++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(lowestX - 1, y, z);
                            direction = Vector3.right;

                            if (Physics.Raycast(origin, direction, out hit, 100))
                            {
                                leftPlatforms.Add(hit.collider.gameObject);
                                if (leftPlatforms[0].transform.position.x > hit.collider.gameObject.transform.position.x)
                                {
                                    leftPlatforms.Insert(0, hit.collider.gameObject);
                                    leftPlatforms.RemoveAt(leftPlatforms.Count-1);
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
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
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            // add to the que of rotations
            if(Input.GetKeyDown(KeyCode.Q)) { rotateQue.Add('Q'); }
            else { rotateQue.Add('E'); }
            playerScript.notRotating = false;
        }
    }

    /// <summary>
    /// If there are rotations still in the que and the camera is not rotating, perform a rotation
    /// </summary>
    void StartRotation()
    {
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
                Debug.Log(currentRotationStep);
                // changes what direction is to the right
                switch (currentRotationStep % 4)
                {
                    case 0:
                        playerScript.forward = new Vector3(1, 0, 0);
                        newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, lowestZ);
                        playerScript.side = 0;
                        //Debug.Log("Front: " + highestZ);
                        break;
                    case -1:
                    case 3:
                        playerScript.forward = new Vector3(0, 0, 1);
                        playerScript.side = 1;
                        newPlayerPos = new Vector3(highestX, player.transform.position.y, player.transform.position.z);
                        //Debug.Log("Right: " + highestX);
                        break;
                    case 2:
                    case -2:
                        playerScript.forward = new Vector3(-1, 0, 0);
                        playerScript.side = 2;
                        newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, highestZ);
                        //Debug.Log("Back: " + lowestZ);
                        break;
                    case 1:
                    case -3:
                        playerScript.forward = new Vector3(0, 0, -1);
                        playerScript.side = 3;
                        newPlayerPos = new Vector3(lowestX, player.transform.position.y, player.transform.position.z);
                        break;
                    default:
                        Debug.Log("Default because: " + currentRotationStep % 4);
                        break;
                }

                pos = new Vector3(/*15 * Mathf.Sin(yTime * Mathf.PI)*/Mathf.Lerp(xStart, xEnd, xTime), Mathf.Lerp(yStart, yEnd, yTime), 0);
                cameraPivot.transform.rotation = Quaternion.Euler(pos.x, pos.y, 0);
                player.transform.rotation = Quaternion.Euler(pos.x, pos.y, 0);

                xStart = OGxStart;
                xEnd = OGxEnd;

                playerScript.SetPosition(newPlayerPos);
            }
            
        }


    }
}


