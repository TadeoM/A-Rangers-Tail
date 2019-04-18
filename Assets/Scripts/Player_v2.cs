using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_v2 : Creature_v2 {

    bool jump;
    int timesJumped;
    int maxJumps;
    float jumpForce = 500;
    float coolDown = 0.15f;
    float specialTimer;
    float specialCD = 3.0f;
    public bool notRotating;
    public Collider swordHitBox;
    private int staminaPoints;
    private bool invincible;
    private bool attacking;
    private float attkTimer;
    private bool special;
    private float invisTimer;
    private float lerpTime;
    int iStart = 0;
    int iEnd = 1;
    Animator playerAnimator;
    Rigidbody rbdy;
    public enum CharacterState
    {
        Idle,
        Run,
        Attack,
        Jump,
        Fall,
        Death
    }

    public CharacterState currentCharState;
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
        ChangeState(CharacterState.Idle);
        //coolDown = 0.75f;
        swordHitBox.enabled = false;
        playerAnimator = GetComponent<Animator>();
        rbdy = GetComponent<Rigidbody>();
        attacking = false;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(notRotating)
        {
            MouseCheck();
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

        if (rbdy.velocity.y < 0)
        {
            ChangeState(CharacterState.Fall);
        }

    }

    void KeyboardCheck()
    {
        
        
        if (Input.GetKey(KeyCode.D))
        {
            direction = forward;
            if (!facingRight && (side == 0 || side == 1))
            {
                Flip();
            }
            else if(!facingRight && (side == 2 || side == 3))
            {
                Flip();
            }
            if (Mathf.Round(direction.x) != 0)
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, 0);
            }
            else
            {
                direction = forward;
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
            }
            if (currentCharState != CharacterState.Attack)
                ChangeState(CharacterState.Run);
            Move(false);
           
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -forward;

           if(facingRight && (side == 0 || side == 1))
            {
                Flip();
            }
            else if (facingRight && (side == 2 || side ==3))
            {
                Flip();
            }
            if (Mathf.Round(direction.x) != 0)
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, 0);
            }
            else
            {
                m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
            }
            if (currentCharState != CharacterState.Attack)
                ChangeState(CharacterState.Run);
            Move(false);
           

        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            if (currentCharState != CharacterState.Attack)
                ChangeState(CharacterState.Idle);
            Move(true);
           
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
            ChangeState(CharacterState.Jump);
        }
    }
    void MouseCheck()
    {
        //Attacking
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attacking && !special)
        {
            attacking = true;
            attkTimer = coolDown;
           
            ChangeState(CharacterState.Attack);
        }

        if (attacking)
        {
            if (attkTimer > 0)
            {
                attkTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
               
            }
        }

        if (Input.GetMouseButtonDown(1) && !special)
        {
           
        }

        if (special)
        {
            if (specialCD > 0)
            {
                specialCD -= Time.deltaTime;
            }
            else
            {
                special = false;

                swordHitBox.enabled = false;

            }
        }
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
        if (Health <= 0)
        {
            ChangeState(CharacterState.Death);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void ChangeState(CharacterState newState)
    {
        currentCharState = newState;
        StartCoroutine(newState.ToString() + "State");
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

    IEnumerator IdleState()
    {
        while(currentCharState == CharacterState.Idle)
        {
            playerAnimator.SetInteger("State", 0);
            yield return null;
        }
    }

    IEnumerator RunState()
    {
        while (currentCharState == CharacterState.Run)
        {
            playerAnimator.SetInteger("State", 1);
            yield return null;
        }
    }

    IEnumerator JumpState()
    {
        while (currentCharState == CharacterState.Jump)
        {
            playerAnimator.SetInteger("State", 2);
            yield return null;
        }
    }

    IEnumerator FallState()
    {
        while (currentCharState == CharacterState.Fall)
        {
            playerAnimator.SetInteger("State", 3);
            if(grounded)
            {
                ChangeState(CharacterState.Idle);
            }
            yield return null;
        }
    }
    IEnumerator AttackState()
    {
        while (currentCharState == CharacterState.Attack)
        {
            playerAnimator.SetInteger("State", 4);
            swordHitBox.enabled = true;
            yield return new WaitForSeconds(0.45f);
            ChangeState(CharacterState.Idle);
            swordHitBox.enabled = false;
        }
    }
    IEnumerator DeathState()
    {
        while (currentCharState == CharacterState.Death)
        {
            playerAnimator.SetInteger("State", 5);
            yield return null;
        }
    }
}
