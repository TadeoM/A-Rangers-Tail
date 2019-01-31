﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using System.Linq;
using Devdog.General;

public class Player_v2 : Creature_v2 {

    bool jump;
    int timesJumped;
    int maxJumps;
    float jumpForce = 500;
    public bool notRotating;

    public int  healthStat; //Drag the health definition file in here

    Animator playerAnimator;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxSpeed = 5f;
        direction = new Vector3(1, 0, 0);
        JumpStrength = 1f;
        maxJumps = 1;
        timesJumped = 0;
        airControl = true;
        jump = false;
        notRotating = true;
        //coolDown = 0.75f;
        //playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        jump = false;
        var myPlayer = PlayerManager.instance.currentPlayer.inventoryPlayer;

    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(notRotating)
        {
            KeyboardCheck();
        }
        else
        {
            m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        }

        if (Grounded)
        {
            jump = false;
            timesJumped = 0;
            playerAnimator.SetBool("IsPlayerJump", jump);
        }
        playerAnimator.SetFloat("Speed", Mathf.Abs(m_Rigidbody.velocity.x));
        Debug.Log(m_Rigidbody.velocity.x);
    }

    void KeyboardCheck()
    {
        /*
        bool flipSprite = (playerRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < 0.01f));
        if(flipSprite)
        {
            playerRenderer.flipX = !playerRenderer.flipX;
        }
        */
        // direction = new Vector3(-direction.x, direction.y, -direction.z);
        if (Input.GetKey(KeyCode.D))
        {
            direction = forward;
            if (Mathf.Round(direction.x) != 0)
            {
                velocity.z = 0;
            }
            else
            {
                direction = forward;
                velocity.x = 0;
            }
            //Move();
            Move(false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -forward;
            if (Mathf.Round(direction.x) != 0)
            {
                velocity.z = 0;
            }
            else
            {
                velocity.x = 0;
            }
            //Move();
            Move(false);
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            //Move(-1);
            Move(true);
        }
        /*else if (direction.x == 1 && velocity.x > 0.002f 
            || direction.z == 1 && velocity.z > 0.002f)
        {
            //Move(-1);
            Move(false, false);
        }
        else if (direction.x == -1 && velocity.x < -0.002f
            || direction.z == -1 && velocity.z < -0.002f)
        {
            //Move(-1);
            Move(false, false);
        }
        else if(velocity.magnitude != 0.0f)
        {
            velocity *= 0.0f;
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && timesJumped < maxJumps)
        {
            timesJumped++;
            jump = true;
            timesJumped++;
            playerAnimator.SetBool("IsPlayerJump", jump);
            Jump();
          
        }
        
  
    }

    public void Jump()
    {
        // If the player should jump...
        if (Grounded && jump) // && m_Anim.GetBool("Ground")
        {
            // Add a vertical force to the player.
            m_Rigidbody.constraints = RigidbodyConstraints.None;
            m_Rigidbody.freezeRotation = true;
            Grounded = false;
            m_Rigidbody.AddForce(new Vector3(0f, jumpForce));
            
            jump = false;

        }
    }
    private void Die()
    {
        //Logic to make the player die
    }

}
