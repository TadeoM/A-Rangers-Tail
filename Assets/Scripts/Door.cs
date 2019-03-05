using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public int areaEntrance;

    private void OnTriggerStay(Collider collision)
    {
        if(collision.GetComponentInParent<Player_v2>().gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeArea(areaEntrance);
            }
        }
    }
}
