using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {

    //Movement
    bool running;
    bool jump;
    int maxJump;
    int timesJumped;
    //Attacks
    float attkTimer;
    float coolDown;
    bool isCoolDown;


    public GameObject swordHitBox;
    //Player Animation
    private Animator playerAnimator;
    private SpriteRenderer playerRenderer;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        MaxSpeed = 0.1f;
        acceleration = new Vector3(0.0f, 0.0f, 0.0f);
        direction = new Vector3(1, 0, 0);
        JumpStrength = 0.2f;
        maxJump = 1;
        timesJumped = 0;
        coolDown = 0.75f;
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        jump = false;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        KeyboardCheck();
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
            Accelerate();
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
            jump = true;
            playerAnimator.SetBool("isPlayerJump", jump);
        }
        playerAnimator.SetFloat("Speed", velocity.x);
        Debug.Log(jump);
        Debug.Log(InAir);
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "ground")
        {
            timesJumped = 0;
            InAir = false;
            jump = false;
            playerAnimator.SetBool("isPlayerJump", jump);
            playerAnimator.SetBool("Grounded", InAir);
        }
        if(collision.gameObject.tag=="enemy")
        {

        }
    }

    
}
