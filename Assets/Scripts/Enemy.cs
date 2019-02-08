using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Creature_v2 {

    protected bool foundTarget;
    public GameObject player;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void CheckPlayer()
    {
        Vector3 leftOrRight = player.transform.position - transform.position;
        switch (side)
        {
            case 0:
                // player on right
                if (leftOrRight.x > position.x)
                    GetComponent<SpriteRenderer>().flipX = true;
                // player on left
                else
                    GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 1:
                // player on right
                if (leftOrRight.z > position.z)
                    GetComponent<SpriteRenderer>().flipX = true;
                //  player on left
                else
                    GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 2:
                // player on right 
                if (leftOrRight.x < position.x)
                    GetComponent<SpriteRenderer>().flipX = true;
                // player on left
                else
                    GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 3:
                // player on right
                if (leftOrRight.z < position.z)
                    GetComponent<SpriteRenderer>().flipX = true;
                // player on left
                else
                    GetComponent<SpriteRenderer>().flipX = false;
                break;
            default:
                Debug.Log("Boyyyy, you snuffed up");
                break;
        }
    }

    protected abstract void PerformAttack();
}
