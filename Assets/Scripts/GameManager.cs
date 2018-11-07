using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject cameraPivot;
    public GameObject player;
    public GameObject[] locationOfPlatforms;    // [0] = rotation 0; [1] = rotation -90(90); [2] = rotation -180(180); [3] = rotation 90(270)
    private Player playerScript;
    private int currentRotationStep;
    private int desiredRotation;


    // Use this for initialization
    void Start () {
        playerScript = player.GetComponent<Player>();
        currentRotationStep = 0;
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
                float tempRot = 0;
                cameraPivot.transform.rotation *= Quaternion.Euler(0, tempRot, 0);
                player.transform.rotation *= Quaternion.Euler(0, tempRot, 0);
                tempRot += 1 * Time.deltaTime;
                
                currentRotationStep--;
                
                newPlayerPos = player.transform.position;
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

            switch (currentRotationStep)
            {
                case 0:
                    playerScript.forward = new Vector3(1, 0, 0);
                    newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, locationOfPlatforms[currentRotationStep].transform.position.z);
                    break;
                case 2:
                    playerScript.forward = new Vector3(-1, 0, 0);
                    newPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, locationOfPlatforms[currentRotationStep].transform.position.z);
                    break;
                case 1:
                    playerScript.forward = new Vector3(0, 0, 1);
                    newPlayerPos = new Vector3(locationOfPlatforms[currentRotationStep].transform.position.x, player.transform.position.y, player.transform.position.z);
                    break;
                case 3:
                    playerScript.forward = new Vector3(0, 0, -1);
                    newPlayerPos = new Vector3(locationOfPlatforms[currentRotationStep].transform.position.x, player.transform.position.y, player.transform.position.z);
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
