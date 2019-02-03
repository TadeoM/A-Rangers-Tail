using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_v2 : MonoBehaviour {

    public enum CreatureType { Bug,Weasel };

    public CreatureType creatureType;
    public LayerMask whatIsGround = 9;

    protected Rigidbody m_Rigidbody;
    protected Transform m_GroundCheck;
    protected Transform m_CeilingCheck;   // A position marking where to check for ceilings
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 axis;
    public Vector3 forward;
    public int side;
    private float maxSpeed;
    private float gravity;
    private float jumpStrength;
    private int health;
    private bool grounded;
    public bool airControl;
    private bool facingRight = true;  // For determining which way the player is currently facing.

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }
    public float JumpStrength
    {
        get { return jumpStrength; }
        set { jumpStrength = value; }
    }
    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    // Use this for initialization
    public virtual void Start () {
        // set values 
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        forward = new Vector3(1, 0, 0);
        health = 0;
        jumpStrength = 0;
        maxSpeed = 5.0f;
        gravity = 0.01f;
        grounded = false;
        jumpStrength = 0;
        position = transform.position;
        m_Rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void FixedUpdate()
    {
        grounded = false;
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        m_Rigidbody.freezeRotation = true;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider[] colliders;
        colliders = Physics.OverlapSphere(m_GroundCheck.position, .02f, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                m_Rigidbody.freezeRotation = true;
            }
                
        }
        //m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
       // m_Anim.SetFloat("vSpeed", m_Rigidbody.velocity.y);
    }

    public virtual void Move(bool slowDown)
    {
        // If crouching, check to see if the character can stand up
        //if (!crouch && m_Anim.GetBool("Crouch"))
        {
            /* If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }*/
        }

        // Set whether or not the character is crouching in the animator
        //m_Anim.SetBool("Crouch", crouch);
        //only control the player if grounded or airControl is turned on
        if (grounded||airControl) // || m_AirControl
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            //move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            //m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody.velocity = new Vector3(direction.x * MaxSpeed, m_Rigidbody.velocity.y, direction.z * MaxSpeed);
            if(slowDown)
            {
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
            }
            //Debug.Log(m_Rigidbody.velocity);

            // If the input is moving the player right and the player is facing left...
            if (direction.x > 0 && !facingRight || direction.z > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (direction.x < 0 && facingRight || direction.z < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }
    public void SetPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    public virtual void TakeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
