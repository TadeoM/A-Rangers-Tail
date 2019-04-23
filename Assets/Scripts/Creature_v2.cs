using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature_v2 : MonoBehaviour {

    public enum CreatureType { Bug,Weasel };

    public CreatureType creatureType;
    public LayerMask whatIsGround = 9;

    public Rigidbody m_Rigidbody;
    public Collider[] colliders;
    public Collider standingOn;
    public Transform m_GroundCheck;
    protected Transform m_CeilingCheck;   // A position marking where to check for ceilings
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 axis;
    public Vector3 forward;
    public int side;
    private float maxSpeed;
    private float gravity;
    private float jumpStrength;
    private int health;
    public bool grounded;
    public bool airControl;
    protected bool facingRight = true;  // For determining which way the player is currently facing.
    

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
        colliders = Physics.OverlapSphere(m_GroundCheck.position, .05f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                // most platforms are oneways, but the bottom-most platforms are not, this is to not allow player to fall through the floor
                if (colliders[i].gameObject.name.Contains("oneway"))
                {
                    if (m_GroundCheck.position.y > -0.05f + colliders[i].gameObject.transform.position.y + (colliders[i].GetComponent<MeshRenderer>().bounds.extents.y))
                    {
                        if (!grounded)
                        {
                            transform.position = new Vector3(transform.position.x, .02f + colliders[i].transform.position.y +
                                (colliders[i].gameObject.GetComponent<BoxCollider>().bounds.extents.y * colliders[i].gameObject.transform.localScale.y), transform.position.z);
                        }
                        grounded = true;
                        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                        m_Rigidbody.freezeRotation = true;
                        standingOn = colliders[i];
                    }
                }
                else if (!colliders[i].gameObject.name.Contains("Door"))
                {
                    if (!grounded) {
                        transform.position = new Vector3(transform.position.x, .019f + colliders[i].transform.position.y + 
                            (colliders[i].gameObject.GetComponent<BoxCollider>().bounds.extents.y * colliders[i].gameObject.transform.localScale.y), transform.position.z);
                    }
                    grounded = true;
                    m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                    m_Rigidbody.freezeRotation = true;
                    standingOn = colliders[i];
                }
            }
        }
    }

    public virtual void Move(bool slowDown)
    {
        // Set whether or not the character is crouching in the animator
        // only control the player if grounded or airControl is turned on
        if (grounded||airControl) // || m_AirControl
        {

            // Move the character
            m_Rigidbody.velocity = new Vector3(direction.x * MaxSpeed, m_Rigidbody.velocity.y, direction.z * MaxSpeed);
            if(slowDown)
            {
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
            }
        }
    }


    protected void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
    }
    public void SetPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    public virtual void TakeDamage(int dmg)
    {
        health -= dmg;
    }
}
