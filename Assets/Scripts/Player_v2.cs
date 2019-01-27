using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_v2 : Creature_v2 {

    bool jump;
    int timesJumped;
    int maxJumps;
    float jumpForce = 500;
    public bool notRotating;

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
        //playerAnimator = GetComponent<Animator>();
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
        }
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
            Move(false);
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            Move(true);
        }
        /*else if (direction.x == 1 && velocity.x > 0.002f 
            || direction.z == 1 && velocity.z > 0.002f)
        {
            Move(false, false);
        }
        else if (direction.x == -1 && velocity.x < -0.002f
            || direction.z == -1 && velocity.z < -0.002f)
        {
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
            Jump();
            //playerAnimator.SetBool("isPlayerJump", jump);
        }
        //playerAnimator.SetFloat("Speed", velocity.x);
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
            //m_Anim.SetBool("Ground", false);
            m_Rigidbody.AddForce(new Vector3(0f, jumpForce));
            jump = false;
        }
    }
}
