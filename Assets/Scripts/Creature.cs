using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 axis;
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
        //facing = (position - previousPosition).normalized;
        if ((velocity + acceleration).sqrMagnitude < velocity.sqrMagnitude && slowDown > 0)
        {
            Debug.Log("Hit this place");
            velocity *= 0.9f;
            velocity.y *= 1 / 0.9f;
        }


        velocity += acceleration;

        if (velocity.magnitude > MaxSpeed)
            velocity = MaxSpeed * direction;
    }
    void Move()
    {
        Falling();
        position += velocity;
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
        if (InAir)
        {
            position.y += velocity.y;
            if (velocity.y < -0.2f)
                velocity.y = -0.15f;
        }
    }

    public void RotateBody(int direction = 1)
    {
        transform.Rotate(new Vector2(direction * 90.0f, 0.0f));
    }

}
