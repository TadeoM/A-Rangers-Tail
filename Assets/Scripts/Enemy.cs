using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Creature_v2 {

    protected bool foundTarget;
    public GameObject player;

    protected void CheckPlayer()
    {
        Vector3 leftOrRight = player.transform.position - transform.position;
        switch (side)
        {
            case 0:
                // player on right
                if (leftOrRight.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                // player on left
                else if (leftOrRight.x < transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1);
                break;
            case 1:
                // player on right
                if (leftOrRight.z > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                //  player on left
                else if (leftOrRight.z < 0)
                    transform.localScale = new Vector3(1, 1, 1);
                break;
            case 2:
                // player on right 
                if (leftOrRight.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                // player on left
                else if (leftOrRight.x > 0)
                    transform.localScale = new Vector3(1, 1, 1);
                break;
            case 3:
                // player on right
                if (leftOrRight.z < 0)
                    transform.localScale = new Vector3(1, 1, 1);
                // player on left
                else if (leftOrRight.z > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                break;
            default:
                Debug.Log("Boyyyy, you snuffed up");
                break;
        }
    }

    protected abstract void PerformAttack();
}
