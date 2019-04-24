using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCalculations : MonoBehaviour {

    public List<Transform> uncheckedPlatforms;    // [0] = rotation 0; [1] = rotation -90(90); [2] = rotation -180(180); [3] = rotation 90(270)
    public List<GameObject> frontPlatforms;
    public List<GameObject> rightPlatforms;
    public List<GameObject> backPlatforms;
    public List<GameObject> leftPlatforms;

    public int  highestX;
    public int  lowestX;
    public int  highestZ;
    public int  lowestZ;
    public int  lowestY;
    public int  highestY;

    // Use this for initialization
    void Start () {
        foreach (Transform child in transform)
        {
            uncheckedPlatforms.Add(child);
        };
        highestX = (int)uncheckedPlatforms[0].position.x;
        lowestX  = (int)uncheckedPlatforms[0].position.x;
        highestZ = (int)uncheckedPlatforms[0].position.z;
        lowestZ  = (int)uncheckedPlatforms[0].position.z;
        lowestY  = (int)uncheckedPlatforms[0].position.y;
        highestY = (int)uncheckedPlatforms[0].position.y;

        GetExtremities();
        PopulatePlatforms();
    }

    public void GetExtremities()
    {
        // go through each tile to check if it is the highest in the X and Z or the lowest
        for (int i = 1; i < uncheckedPlatforms.Count; i++)
        {            
            Vector3 currentLoc = uncheckedPlatforms[i].position;
            if (currentLoc.x > highestX)
            {
                highestX = (int)uncheckedPlatforms[i].position.x;
            }
            else if (currentLoc.x < lowestX)
            {
                lowestX = (int)uncheckedPlatforms[i].position.x;
            }
            if (currentLoc.z > highestZ)
            {
                highestZ = (int)uncheckedPlatforms[i].position.z;
            }
            else if (currentLoc.z < lowestZ)
            {
                lowestZ = (int)uncheckedPlatforms[i].position.z;
            }
            if (currentLoc.y > highestY)
            {
                highestY = (int)uncheckedPlatforms[i].position.y;
            }
            else if (currentLoc.y < lowestY)
            {
                lowestY = (int)uncheckedPlatforms[i].position.y;
            }
        }
    }

    public void PopulatePlatforms()
    {
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
                                if (uncheckedPlatforms.Contains(hit.transform))
                                {
                                    if (hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 12)
                                    {
                                        frontPlatforms.Add(hit.collider.gameObject);
                                    }
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
                                if(uncheckedPlatforms.Contains(hit.transform))
                                {
                                    if (hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 12)
                                    {
                                        rightPlatforms.Add(hit.collider.gameObject);
                                    }
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
                                if (uncheckedPlatforms.Contains(hit.transform))
                                {
                                    if (hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 12)
                                    {
                                        backPlatforms.Add(hit.collider.gameObject);
                                    }
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
                                if (uncheckedPlatforms.Contains(hit.transform))
                                {
                                    if (hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 12)
                                    {
                                        leftPlatforms.Add(hit.collider.gameObject);
                                    }
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

    
}
