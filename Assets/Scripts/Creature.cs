using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    // set attributes
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 axis;
    public Vector3 forward;
    private float gravity;
    public Vector3 acceleration;
    public float accelRate;
    private float maxSpeed;
    private float jumpStrength;
    private int health;
    private bool inAir;
    private Vector3 facing;
    private Vector3 previousPosition;

    public Vector3 PreviousPosition
    {
        get { return previousPosition; }
        set { previousPosition = value; }
    }


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
    public bool InAir
    {
        get { return inAir; }
        set { inAir = value; }
    }
    public Vector3 Facing
    {
        get { return facing; }
        set { facing = value; }
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
        inAir = false;
        jumpStrength = 0;
        position = transform.position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Move();
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
        acceleration = accelRate * direction * slowDown;
        // if the creature is slowing down, make it slow down faster
        if ((velocity + acceleration).sqrMagnitude < velocity.sqrMagnitude && slowDown > 0)
        {
            velocity *= 0.9f;
            //velocity.y *= 1 / 0.9f;
        }

        velocity += acceleration;
        
        // clamp speed to max speed
        if (velocity.magnitude > MaxSpeed)
            velocity = MaxSpeed * direction;
    }

    /// <summary>
    /// called every frame
    /// Check if falling, and increase position by the velocity
    /// </summary>
    void Move()
    {
        Falling();
        position += velocity;
        if(InAir)
        {
            //position.y -= 0.05f;
        }
    }

    /// <summary>
    /// triggers if creatures is going to jump
    /// </summary>
    public void Jump()
    {
        velocity.y = JumpStrength;
        InAir = true;
    }

    /// <summary>
    /// if the creature is above ground 
    /// </summary>
    public void Falling()
    {
       
        if (InAir) {
            
            velocity.y -= gravity;
            //velocity.y -= Mathf.Pow(gravity, 2); ;
            if (velocity.y < -0.2f)
                velocity.y = -0.2f;
        }
        if(!InAir) { velocity.y = 0.0f; }
          
    }

    public void RotateBody(int direction = 1)
    {
        transform.Rotate(new Vector2(direction * 90.0f, 0.0f));
    }

    public void SetPosition(Vector3 newPos)
    {
        position = newPos;
    }
}
