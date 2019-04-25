using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_v2 : Creature_v2 {

    bool jump;
    int timesJumped;
    int maxJumps;
    public float dbljumpForce = 150;
    public float rollTime;
    public float startRollTime;
    public float rollSpeed;
    public float rollCD;
    float specialTimer;
    float specialCD = 3.0f;
    public bool notRotating;
    public float cooldown;
    public Collider swordHitBox;
    public Collider bodyHitBox;
    private int staminaPoints;
    private bool invincible;
    private bool attacking;
    private float attkTimer;
    public float waitTimer;
    private bool special;
    private float invisTimer;
    private float lerpTime;
    int iStart = 0;
    int iEnd = 1;
    public int combo = 0;
    public int chainedHits;
    public float[] attackDuration;
    public int comboIndex;
    Animator playerAnimator;
    public int missionObj;
    public enum CharacterState
    {
        Idle,
        Run,
        Attack,
        Jump,
        Fall,
        Roll,
        Death
    }

    public enum attack
    {
       Idle,
       Attacking,
       Waiting
    }

    public attack currAttackstate;
    public CharacterState currentCharState;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxSpeed = 3f;
        direction = new Vector3(1, 0, 0);
        JumpStrength = 230f;
        maxJumps = 2;
        timesJumped = 0;
        airControl = true;
        jump = false;
        notRotating = true;
        Health = 5;
        combo = 0;
        chainedHits = 0;
        currentCharState = CharacterState.Idle;
        currAttackstate = attack.Idle;
        //coolDown = 0.75f;
        swordHitBox.enabled = false;
        playerAnimator = GetComponent<Animator>();
        attacking = false;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(notRotating)
        {
            KeyboardCheck();
            MouseCheck();
            
            
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

        if (m_Rigidbody.velocity.y < 0)
        {
            ChangeState(CharacterState.Fall);
        }
        else if (currentCharState == CharacterState.Jump && grounded)
        {
            ChangeState(CharacterState.Idle);
        }
        ResetGame();
    }

    void KeyboardCheck()
    {        
        if (Input.GetKey(KeyCode.D))
        {
            direction = forward;
            if (!facingRight)
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
            if (currentCharState != CharacterState.Attack && currentCharState != CharacterState.Jump && currentCharState != CharacterState.Fall)
            {
                ChangeState(CharacterState.Run);
                Move(false);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -forward;

           if(facingRight)
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
            if (currentCharState != CharacterState.Attack && currentCharState != CharacterState.Jump && currentCharState != CharacterState.Fall)
            {
                ChangeState(CharacterState.Run);
                Move(false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            Move(true);
           
        }
        
        else
        {
           
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x * 0.67f, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z * 0.67f);
        }
        
        if(Input.GetKeyDown(KeyCode.LeftShift) && Grounded && rollCD<0)
        {
            Debug.Log("I pressed shift question mark");
           
                if(direction == -forward)
                {
                    Debug.Log("Roll input");
                  
                    ChangeState(CharacterState.Roll);
                    m_Rigidbody.velocity = new Vector3(0, 0, (m_Rigidbody.velocity.z * rollSpeed)+1.0f);
                
                }
                else if(direction == forward)
                {
                    Debug.Log("Roll input");
                
                    ChangeState(CharacterState.Roll);
                    m_Rigidbody.velocity = new Vector3((m_Rigidbody.velocity.x * rollSpeed)+1.0f, 0, 0);
                }
           
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            ChangeState(CharacterState.Fall);
            Jump(true);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && timesJumped < maxJumps)
        {
            ChangeState(CharacterState.Jump);
            timesJumped++;
            jump = true;
            Jump(false);
            
        }
        if(new Vector3(0.001f,0.0f,0.001f).magnitude>m_Rigidbody.velocity.magnitude)
        {
            if (currentCharState != CharacterState.Attack && Grounded == true) 
                ChangeState(CharacterState.Idle);
        }
        rollCD -= Time.deltaTime;
    }
    void MouseCheck()
    {
        //Attacking
        switch(currAttackstate)
        {
            case attack.Idle:
                if (Input.GetKeyDown(KeyCode.Mouse0) && !special)
                {
                    comboIndex = 0;
                    currAttackstate = attack.Attacking;
                    attkTimer = attackDuration[0];
                    ChangeState(CharacterState.Attack);
                    chainedHits = 1;
                }
                break;
            case attack.Attacking:
                attkTimer -= Time.deltaTime;

                if(attkTimer<0)
                {
                    attkTimer = waitTimer;
                    currAttackstate = attack.Waiting;
                }
                break;

            case attack.Waiting:
                attkTimer -= Time.deltaTime;

                if(attkTimer<0)//ran out of time to chain combo
                {
                    comboIndex = 0;
                    currAttackstate = attack.Idle;
                    ChangeState(CharacterState.Idle);
                }

                if(Input.GetMouseButton(0))//continue attacking
                {
                    comboIndex++;//Go to next attack animation

                    if(comboIndex>=attackDuration.Length)//Check if the combo is over, start a new combo
                    {
                        comboIndex = 0;
                        currAttackstate = attack.Idle;
                        //Tell animation to play first attack
                        ChangeState(CharacterState.Attack);
                        chainedHits++;

                    }

                    else
                        {
                            currAttackstate = attack.Attacking;
                            attkTimer = attackDuration[comboIndex];
                        ChangeState(CharacterState.Attack);
                            chainedHits++;
                        }
                    Debug.Log("In combo Attack, Index: "+comboIndex);
                }
                break;
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
          
            if (timesJumped == maxJumps)
                m_Rigidbody.AddForce(new Vector3(0, dbljumpForce));
            else
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

    protected override void OnDeath()
    {
        //ChangeState(CharacterState.Death);
        int sceneNum = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNum);
    }

    public void comboCounter()
    {

    }
    void ChangeState(CharacterState newState)
    {
        currentCharState = newState;
        StartCoroutine(newState.ToString() + "State");
    }
    private void OnCollisionEnter(Collision other)
    {
     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && !invincible)
        {
            TakeDamage(1);
            invisTimer = 60 * Time.deltaTime;
        }

        if(other.gameObject.layer ==13)
        {
            Debug.Log("Collide with Object");
            missionObj++;
        }
        /*
        if (other.gameObject.layer == 11 && !invincible)
        {
            TakeDamage(1);
            invisTimer = 60 * Time.deltaTime;
        }
        */
    }

    private void ResetGame()
    {
        if(missionObj==3)
        {
            OnDeath();
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
            if(Grounded)
            {
                ChangeState(CharacterState.Idle);
            }
            yield return null;
        }
    }
    IEnumerator RollState()
    {
        while (currentCharState == CharacterState.Roll)
        {
            Debug.Log("Roll Duration");
            playerAnimator.SetInteger("State", 7);
            rollCD = 15.0f;
            yield return new WaitForSeconds(0.7f);
        }
    }
    IEnumerator AttackState()
    {
        while (currentCharState == CharacterState.Attack)
        {
            if(comboIndex==0)
            {
                playerAnimator.SetInteger("State", 4);
                swordHitBox.enabled = true;
                yield return new WaitForSeconds(attkTimer);
                swordHitBox.enabled = false;
                Debug.Log("In Attack 1");
            }
            else if(comboIndex==1)
            {
                playerAnimator.SetInteger("State", 5);
                swordHitBox.enabled = true;
                yield return new WaitForSeconds(attkTimer);
                swordHitBox.enabled = false;
                Debug.Log("In Attack 2");
            }
            else if (comboIndex == 2)
            {
                playerAnimator.SetInteger("State", 6);
                swordHitBox.enabled = true;
                yield return new WaitForSeconds(attkTimer);
                swordHitBox.enabled = false;
                Debug.Log("In Attack 3");
            }

        }
    }
    IEnumerator DeathState()
    {
        while (currentCharState == CharacterState.Death)
        {
            playerAnimator.SetInteger("State", 10);
            yield return null;
        }
    }


}
