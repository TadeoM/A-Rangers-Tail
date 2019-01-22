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
        MaxSpeed = 10f;
        acceleration = new Vector3(0.0f, 0.0f, 0.0f);
        direction = new Vector3(1, 0, 0);
        JumpStrength = 1f;
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
    private void FixedUpdate()
    {
        M_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider[] colliders = Physics.OverlapBox(position, GetComponent<BoxCollider>().size, Quaternion.identity, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                M_Grounded = true;
        }
        //animator.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        //m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }

    public override void Move()
    {
        base.Move();
        if (M_Grounded && jump )
        {
            // Add a vertical force to the player.
            M_Grounded = false;
            //m_Anim.SetBool("Ground", false);
            creature.AddForce(new Vector3(0f, JumpStrength));
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
            Accelerate(-1);
        }
        else if (direction.x == -1 && velocity.x < -0.002f
            || direction.z == -1 && velocity.z < -0.002f)
        {
            Accelerate(-1);
        }
        else if(velocity.magnitude != 0.0f)
        {
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
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "ground")
        {
            timesJumped = 0;
            M_Grounded = false;
            jump = false;
            playerAnimator.SetBool("isPlayerJump", jump);
            playerAnimator.SetBool("Grounded", M_Grounded);
        }
        if(collision.gameObject.tag=="enemy")
        {

        }
    }
}
