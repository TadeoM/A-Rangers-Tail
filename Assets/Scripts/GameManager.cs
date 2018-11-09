﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject cameraPivot;
    public GameObject player;
    public GameObject[] unCheckedlatforms;    // [0] = rotation 0; [1] = rotation -90(90); [2] = rotation -180(180); [3] = rotation 90(270)
    public List<GameObject> frontPlatforms;
    public List<GameObject> rightPlatforms;
    public List<GameObject> backPlatforms;
    public List<GameObject> leftPlatforms;
    private Player playerScript;
    private int currentRotationStep;
    private int desiredRotation;

    // Use this for initialization
    void Start () {
        
        playerScript = player.GetComponent<Player>();
        currentRotationStep = 0;
        unCheckedlatforms = GameObject.FindGameObjectsWithTag("ground");
        Vector3 firstObject = transform.TransformPoint(unCheckedlatforms[0].GetComponent<Transform>().position);
        Debug.Log(player.layer);
        // set all highest and lowest points to first box
        int highestX = (int)firstObject.x;
        int lowestX = (int)firstObject.x;
        int highestZ =(int)firstObject.z;
        int lowestZ = (int)firstObject.z;
        int lowestY = (int)firstObject.y;
        int highestY = (int)firstObject.y;

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

                            if (Physics.Raycast(origin, direction, out hit, 100, player.layer))
                            {
                                frontPlatforms.Add(hit.collider.gameObject);
                            }
                        }
                    }
                    break;
                case 1:
                    Debug.Log(lowestZ);
                    for (int z = lowestZ; z <= highestZ; z++)
                    {
                        for (int y = lowestY; y <= highestY; y++)
                        {
                            origin = new Vector3(highestX + 1, y, z);
                            direction = Vector3.left;

                            if (Physics.Raycast(origin, direction, out hit, 100, player.layer))
                            {
                                rightPlatforms.Add(hit.collider.gameObject);
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

                            if (Physics.Raycast(origin, direction, out hit, 100, player.layer))
                            {
                                backPlatforms.Add(hit.collider.gameObject);
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

                            if (Physics.Raycast(origin, direction, out hit, 100, player.layer))
                            {
                                leftPlatforms.Add(hit.collider.gameObject);
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
	void Update () {
        KeyboardCheck();
        RotateScene();
    }

    void KeyboardCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            Vector3 newPlayerPos;
            // Rotate the camera to the left when Q is pressed
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //float tempRot = 0;
                cameraPivot.transform.rotation *= Quaternion.Euler(0, 90, 0);
                player.transform.rotation *= Quaternion.Euler(0, 90, 0);
                //tempRot += 1 * Time.deltaTime;
                currentRotationStep--;
                if (currentRotationStep < 0)
                {
                    currentRotationStep = 3;
                }
            }
            // Rotate the camera to the right when E is pressed
            else
            {
                cameraPivot.transform.rotation *= Quaternion.Euler(0, -90, 0);
                player.transform.rotation *= Quaternion.Euler(0, -90, 0);
                currentRotationStep++;
            }

            currentRotationStep %= 4;
            newPlayerPos = player.transform.position;
            Debug.Log(currentRotationStep);


            switch (currentRotationStep)
            {
                case 0:
                    playerScript.forward = new Vector3(1, 0, 0);
                    newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, unCheckedlatforms[currentRotationStep].transform.position.z);
                    break;
                case 1:
                    playerScript.forward = new Vector3(0, 0, 1);
                    newPlayerPos = new Vector3(unCheckedlatforms[currentRotationStep].transform.position.x, player.transform.position.y, player.transform.position.z);
                    break;
                case 2:
                    playerScript.forward = new Vector3(-1, 0, 0);
                    newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, unCheckedlatforms[currentRotationStep].transform.position.z);
                    break;
                case 3:
                    playerScript.forward = new Vector3(0, 0, -1);
                    newPlayerPos = new Vector3(unCheckedlatforms[currentRotationStep].transform.position.x, player.transform.position.y, player.transform.position.z);
                    break;
                default:
                    break;
            }
            playerScript.SetPosition(newPlayerPos);
        }
        
    }
    void RotateScene()
    {

    }
}
