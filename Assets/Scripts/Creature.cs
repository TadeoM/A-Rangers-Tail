using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    // a creature will have type of creature, and have a certain amount of weaknesses!
    public enum CreatureType { };

    public CreatureType creatureType;
    public LayerMask whatIsGround = 9;

    // set attributes
    public Rigidbody creature;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 axis;
    public Vector3 forward;
    public Vector3 acceleration;
    public float accelRate;
    private float maxSpeed;
    private float gravity;
    private float jumpStrength;
    private int health;
    private bool m_Grounded;

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
    public bool M_Grounded
    {
        get { return m_Grounded; }
        set { m_Grounded = value; }
    }

    // Use this for initialization
    public virtual void Start()
    {
        // set values 
        forward = new Vector3(1, 0, 0);
        health = 0;
        jumpStrength = 0;
        maxSpeed = 0.0f;
        gravity = 0.01f;
        m_Grounded = false;
        jumpStrength = 0;
        position = transform.position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //Move();
        GetComponent<Rigidbody>().MovePosition(position);
        //Debug.Log(velocity);
    }

    /// <summary>
    /// moves creature in direction at whatever current speed
    /// direction should be 1 or -1
    /// slowDown will be 1 unless the player must slow down
    /// </summary>
    /// <param name="direction"></param>
    public void Accelerate(int slowDown = 1)
    {
        /*(acceleration = accelRate * direction * slowDown;
        // if the creature is slowing down, make it slow down faster
        if ((velocity + acceleration).sqrMagnitude < velocity.sqrMagnitude && slowDown > 0)
        {
            velocity *= 0.9f;
            //velocity.y *= 1 / 0.9f;
        }

        velocity += acceleration;
        
        // clamp speed to max speed
        if (velocity.magnitude > MaxSpeed)
            velocity = MaxSpeed * direction;*/
        Move();
    }

    /// <summary>
    /// called every frame
    /// Check if falling, and increase position by the velocity
    /// </summary>
    public virtual void Move()
    {
        Falling();
        position += velocity;
        if(M_Grounded)
        {
            //position.y -= 0.05f;
        }

        if (m_Grounded)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier

            // The Speed animator parameter is set to the absolute value of the horizontal input.
           // m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            creature.velocity = new Vector3( direction.x * maxSpeed, creature.velocity.y, direction.z * maxSpeed);

            // If the input is moving the player right and the player is facing left...
            //if (direction.x > 0 && !m_FacingRight)
            //{
            //    // ... flip the player.
            //    Flip();
            //}
            // Otherwise if the input is moving the player left and the player is facing right...
            //else if (move < 0 && m_FacingRight)
            //{
            //    // ... flip the player.
            //    Flip();
            //}
        }
        // If the player should jump...
        
    }

    /// <summary>
    /// triggers if creatures is going to jump
    /// </summary>
    public void Jump()
    {
        velocity.y = JumpStrength;
        M_Grounded = true;
    }

    /// <summary>
    /// if the creature is above ground 
    /// </summary>
    public void Falling()
    {
       
        if (!M_Grounded) {
            
            velocity.y -= gravity;
            //velocity.y -= Mathf.Pow(gravity, 2); ;
            if (velocity.y < -0.2f)
                velocity.y = -0.2f;
        }
        if(M_Grounded) { velocity.y = 0.0f; }
          
    }

    public void RotateBody(int direction = 1)
    {
        transform.Rotate(new Vector2(direction * 90.0f, 0.0f));
    }

    public void SetPosition(Vector3 newPos)
    {
        position = newPos;
    }
    public virtual void DealDamage(int dmg, CreatureType type)
    {

    }

    public virtual void TakeDamage(int dmg, CreatureType type)
    {

    }
}
