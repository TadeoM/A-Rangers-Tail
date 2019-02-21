using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cameraPivot;
    public GameObject player;
    public int currentArea;
    public List<AreaCalculations> areaScripts;
    public float yStart;
    public float yEnd;
    public int currentRotationStep;
    private List<char> rotateQue = new List<char>();
    private int rotateCount;
    private Player_v2 playerScript;
    private int desiredRotation;    // where we should be rotating to 
    private bool rotateNeeded; // not used - how many rotations need to be done
    static private float yTime;
    static private float xTime;

    public float xStart;
    public float xEnd;
    public float OGxStart;
    public float OGxEnd;
    public bool swapped;

    // make a script in each area
        // each area will calculate its children and check which ones would be on what side
    // this script will grab all the areas and make sure the player is rotating through the area they are in.
        // disable any area that the player is not in
    // 

    // Use this for initialization
    void Start()
    {
        playerScript = player.GetComponent<Player_v2>();
        currentRotationStep = 0;
        // set all highest and lowest points to first box
        yStart = 0.0f;
        yEnd = 0.0f;
        rotateNeeded = false;
        yTime = 0.0f;
        xTime = 0.0f;
        xStart = 0.0f;
        xEnd = 15.0f;
        OGxStart = 0.0f;
        OGxEnd = 15.0f;

        foreach (var area in areaScripts)
        {
            foreach (var notAreas in areaScripts)
            {
                area.enabled = false;
            }
            area.enabled = true;
            foreach (Transform child in area.transform)
            {
                area.uncheckedPlatforms.Add(child);
            };
            area.GetExtremities();
            area.PopulatePlatforms();
        }

        areaScripts[currentArea].enabled = true;

        //grab the front platforms and move them to the front
        foreach (var platform in areaScripts[currentArea].frontPlatforms)
        {
            float zPos = areaScripts[currentArea].lowestZ - platform.transform.position.z;
            Vector3 platformPos = platform.GetComponent<BoxCollider>().center;
            platformPos = new Vector3(0, 0, zPos);
            platform.GetComponent<BoxCollider>().center = platformPos;
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
                        playerScript.side = 0;
                        playerScript.forward = new Vector3(1, 0, 0);
                        newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, areaScripts[currentArea].lowestZ);
                        foreach (var platform in areaScripts[currentArea].rightPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].backPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].leftPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].frontPlatforms)
                        {
                            float zPos = areaScripts[currentArea].lowestZ - platform.transform.position.z;
                            Vector3 platformPos = platform.GetComponent<BoxCollider>().center;
                            platformPos = new Vector3(0, 0, zPos);
                            platform.GetComponent<BoxCollider>().center = platformPos;
                        }
                        break;
                    case -1:
                    case 3:
                        playerScript.side = 1;
                        playerScript.forward = new Vector3(0, 0, 1);
                        newPlayerPos = new Vector3(areaScripts[currentArea].highestX, player.transform.position.y, player.transform.position.z);
                        foreach (var platform in areaScripts[currentArea].frontPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].backPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].leftPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].rightPlatforms)
                        {
                            float xPos = areaScripts[currentArea].highestX - platform.transform.position.x;
                            Vector3 platformPos = platform.GetComponent<BoxCollider>().center;
                            platformPos = new Vector3(xPos, 0, 0);
                            platform.GetComponent<BoxCollider>().center = platformPos;
                        }
                        break;
                    case 2:
                    case -2:
                        Debug.Log("Case -2 or 2");
                        playerScript.side = 2;
                        playerScript.forward = new Vector3(-1, 0, 0);
                        newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, areaScripts[currentArea].highestZ);
                        
                        foreach (var platform in areaScripts[currentArea].rightPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].frontPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].leftPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].backPlatforms)
                        {
                            float zPos = areaScripts[currentArea].highestZ - platform.transform.position.z;
                            Vector3 platformPos = platform.GetComponent<BoxCollider>().center;
                            platformPos = new Vector3(0, 0, zPos);
                            platform.GetComponent<BoxCollider>().center = platformPos;
                        }
                        break;
                    case 1:
                    case -3:
                        Debug.Log("Case 1 or -3");
                        playerScript.side = 3;
                        playerScript.forward = new Vector3(0, 0, -1);
                        newPlayerPos = new Vector3(areaScripts[currentArea].lowestX, player.transform.position.y, player.transform.position.z);
                        foreach (var platform in areaScripts[currentArea].frontPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].backPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].leftPlatforms)
                        {
                            platform.GetComponent<BoxCollider>().center = Vector3.zero;
                        }
                        foreach (var platform in areaScripts[currentArea].rightPlatforms)
                        {
                            float xPos = areaScripts[currentArea].lowestX - platform.transform.position.x;
                            Vector3 platformPos = platform.GetComponent<BoxCollider>().center;
                            platformPos = new Vector3(xPos, 0, 0);
                            platform.GetComponent<BoxCollider>().center = platformPos;
                        }
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
                rotateCount = 0;
            }
            
        }


    }
    public void ChangeArea(int newArea)
    {
        areaScripts[currentArea].enabled = false;
        currentArea = newArea;
        areaScripts[currentArea].enabled = true;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, areaScripts[currentArea].lowestZ);
    }
}


