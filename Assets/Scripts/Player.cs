using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {

    int maxJump;
    int timesJumped;

	// Use this for initialization
	public override void Start () {
        base.Start();
        MaxSpeed = 0.1f;
        acceleration = new Vector3(0.0f, 0.0f, 0.0f);
        direction = new Vector3(1, 0, 0);
        JumpStrength = 0.2f;
        maxJump = 1;
        timesJumped = 0;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        KeyboardCheck();
	}

    void KeyboardCheck()
    {

        // direction = new Vector3(-direction.x, direction.y, -direction.z);
        if (Input.GetKey(KeyCode.D))
        {
            if (direction.x != 0)
                direction.x = 1;
            else
                direction.z = 1;
            Accelerate();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (direction.x != 0)
                direction.x = -1;
            else
                direction.z = -1;
            Accelerate();
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            Accelerate(-1);
        }
        else if (direction.x == 1 && velocity.x > 0.002f 
            || direction.z == 1 && velocity.z > 0.002f)
        {
            Debug.Log("Slowing Down at 1");
            Accelerate(-1);
        }
        else if (direction.x == -1 && velocity.x < -0.002f
            || direction.z == -1 && velocity.z < -0.002f)
        {
            Debug.Log("Slowing down at -1");
            Accelerate(-1);
        }
        else if(velocity.magnitude != 0.0f)
        {
            Debug.Log(velocity);
            velocity *= 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && timesJumped < maxJump)
        {
            Jump();
            timesJumped++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            timesJumped = 0;
            InAir = false;
        }
    }
}
