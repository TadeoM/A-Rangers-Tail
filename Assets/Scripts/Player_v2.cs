using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_v2 : Creature_v2 {

    bool jump;
    int timesJumped;
    int maxJumps;
    public bool notRotating;
    private int staminaPoints;
    private bool invincible;
    private float invisTimer;
    private float lerpTime;
    int iStart = 0;
    int iEnd = 1;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxSpeed = 3f;
        direction = new Vector3(1, 0, 0);
        JumpStrength = 230f;
        maxJumps = 1;
        timesJumped = 0;
        airControl = true;
        jump = false;
        notRotating = true;
        Health = 5;
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

        if (invisTimer > 0)
        {
            invisTimer -= Time.deltaTime;
            lerpTime += 4 * (Time.deltaTime);
            Color newColor = gameObject.GetComponent<SpriteRenderer>().color;

            if (lerpTime > 1)
            {
                lerpTime = 0;
                int temp = iStart;
                iStart = iEnd;
                iEnd = temp;
            }
            newColor = new Color(newColor.r, newColor.g, newColor.b, Mathf.Lerp(iStart, iEnd, lerpTime));
            gameObject.GetComponent<SpriteRenderer>().color = newColor;
        }
        else
        {
            invincible = false;
            Color newColor = gameObject.GetComponent<SpriteRenderer>().color;
            newColor = new Color(newColor.r, newColor.g, newColor.b, 1);
            gameObject.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    void KeyboardCheck()
    {        
        if (Input.GetKey(KeyCode.D))
        {
            direction = forward;
            if (Mathf.Round(direction.x) != 0)
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, 0);
            }
            else
            {
                direction = forward;
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
            }
            Move(false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -forward;
            if (Mathf.Round(direction.x) != 0)
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, 0);
            }
            else
            {
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
            }
            Move(false);
        }
        else
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * 0.67f, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z * 0.67f);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            Jump(true);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && timesJumped < maxJumps)
        {
            timesJumped++;
            jump = true;
            timesJumped++;
            Jump(false);
        }
        
        //playerAnimator.SetFloat("Speed", velocity.x);
    }

    public void Jump(bool down)
    {
        // If the player should jump...
        if (jump && !down) // && m_Anim.GetBool("Ground")
        {
            // Add a vertical force to the player.
            m_Rigidbody.constraints = RigidbodyConstraints.None;
            m_Rigidbody.freezeRotation = true;
            Grounded = false;
            //m_Anim.SetBool("Ground", false);
            m_Rigidbody.AddForce(new Vector3(0f, JumpStrength));
            jump = false;
        }
        else if (Grounded && down)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.None;
            m_Rigidbody.freezeRotation = true;
            Grounded = false;
            //m_Anim.SetBool("Ground", false);
            m_Rigidbody.AddForce(new Vector3(0f, -JumpStrength));
            jump = false;
        }
    }
    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        invincible = true;
        invisTimer = 3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && !invincible)
        {
            TakeDamage(1);
            invisTimer = 60 * Time.deltaTime;
        }
    }
}
