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
        acceleration = 0.0f;
        accelRate = 0.005f;
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
        if (Input.GetKey(KeyCode.D))
        {
            Move();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(-1);
        }
        if (Input.GetKeyDown(KeyCode.Space) && timesJumped < maxJump)
        {
            Jump();
            timesJumped++;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            velocity = 0.0f;
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
