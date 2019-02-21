using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public int areaEntrance;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.GetComponentInParent<Player_v2>().gameObject.tag);
        if(collision.GetComponentInParent<Player_v2>().gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeArea(areaEntrance);
            }
        }
    }
}
