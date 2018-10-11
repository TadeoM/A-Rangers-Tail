using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public Vector2 position;
    public float velocity;
    private float gravity;
    public float acceleration;
    public float accelRate;
    private float maxSpeed;
    private float jumpVelocity;
    private float speed;
    private float jumpStrength;
    private float health;
    private bool inAir;

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
    public float JumpVelocity
    {
        get { return jumpVelocity; }
        set { jumpVelocity = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float JumpStrength
    {
        get { return jumpStrength; }
        set { jumpStrength = value; }
    }
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public bool InAir
    {
        get { return inAir; }
        set { inAir = value; }
    }

    // Use this for initialization
    public virtual void Start()
    {
        health = 0;
        jumpStrength = 0;
        speed = 0;
        maxSpeed = 0.0f;
        gravity = 0.01f;
        inAir = false;
        jumpStrength = 0;
        position = transform.position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Falling();
        GetComponent<Rigidbody>().MovePosition(position); 
    }

    /// <summary>
    /// moves creature in direction at whatever current speed
    /// direction should be 1 or -1
    /// </summary>
    /// <param name="direction"></param>
    public void Move(int direction = 1)
    {
        acceleration = accelRate * direction;
        velocity += acceleration;
        if ((velocity > MaxSpeed && direction == 1) || (velocity < -MaxSpeed && direction == -1))
        {
            velocity = MaxSpeed * direction;
        }
        position.x += velocity;
    }

    /// <summary>
    /// triggers if creatures is going to jump
    /// </summary>
    public void Jump()
    {
        JumpVelocity = JumpStrength;
        position.y += JumpVelocity;
        InAir = true;
    }

    /// <summary>
    /// if the creature is above ground 
    /// </summary>
    public void Falling()
    {
        if (InAir)
        {
            position.y += JumpVelocity;
            JumpVelocity -= Gravity;
            if (JumpVelocity < -0.2f)
                JumpVelocity = -0.15f;
        }
    }

    public void RotateBody(int direction = 1)
    {
        transform.Rotate(new Vector2(direction * 90.0f, 0.0f));
    }

}
