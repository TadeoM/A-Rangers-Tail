using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void KeyboardCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Rotate Left");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Rotate Right");
        }
    }
}
