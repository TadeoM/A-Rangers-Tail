using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeAI : Enemy {

    enum Attacks
    {
        Lunge, Charge
    }
    private Attacks attackChosen;
    public GameObject lungeCollider;
    public GameObject chargeCollider;
    public GameObject attackCollider;
    public float attackTimer;
    private Animator animator;
    private Vector3 startAttackPos;
    private Vector3 endAttackPos;
    private int timesSwapped;
    private float currTime;
    private float animSpeed;
    private float startHeight;
    private float endHeight;
    private bool inAttackState;

	// Use this for initialization
	public override void Start () {
        base.Start();
        MaxSpeed = 2f;
        switch (side)
        {
            case 0:
                forward  = new Vector3(1, 0, 0);
                break;
            case 1:
                forward = new Vector3(0, 0, 1);
                break;
            case 2:
                forward = new Vector3(-1, 0, 0);
                break;
            case 3:
                forward = new Vector3(0, 0, -1);
                break;
            default:
                break;
        }
        
        creatureType = CreatureType.Bug;
        inAttackState = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        //attackCollider = GetComponentInChildren<CapsuleCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		if(!inAttackState && attackTimer < -(Time.deltaTime * 60))
        {
            if (player.GetComponent<Player_v2>().side == side)
                PerformAttack();
        }
        else if (player.GetComponent<Player_v2>().side == side)
        {
            AdjustColliders();
        }
        else
        {
            StopEverything();
        }

        if(attackTimer <= 0)
        {
            StopEverything();
        }

        attackTimer -= Time.deltaTime;
        CheckPlayer();
    }

    protected override void PerformAttack()
    {
        Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if (Vector3.Distance(player.transform.position, transform.position) <= 3f)
        {
            // needs change
            inAttackState = true;
            attackChosen = Attacks.Lunge;
            animator.SetInteger("State", 2);
            attackTimer = 80 * Time.deltaTime;
            attackCollider = lungeCollider;
            attackCollider.SetActive(true);

            Vector3 leftOrRight = player.transform.position - transform.position;
            switch (side)
            {
                case 0:
                    startAttackPos = new Vector3(-0.14f, 0, 0);
                    endAttackPos = new Vector3(-0.8f, -0.3f, 0);
                    break;
                case 1:
                    // player on right
                    if (leftOrRight.z > 0)
                    {
                        startAttackPos = new Vector3(0, 0, 0.14f);
                        endAttackPos = new Vector3(0, -0.3f, 0.8f);
                    }
                    //  player on left
                    else if (leftOrRight.z < 0)
                    {
                        startAttackPos = new Vector3(0, 0, -0.14f);
                        endAttackPos = new Vector3(0, -0.3f, -0.8f);
                    }
                    break;
                case 2:
                    // player on right 
                    if (leftOrRight.x < 0)
                    {
                        startAttackPos = new Vector3(-0.14f, 0, 0);
                        endAttackPos = new Vector3(-0.8f, -0.3f, 0);
                    }
                    // player on left
                    else if (leftOrRight.x > 0)
                    {
                        startAttackPos = new Vector3(0.14f, 0, 0);
                        endAttackPos = new Vector3(0.8f, -0.3f, 0);
                    }
                    break;
                case 3:
                    // player on right
                    if (leftOrRight.z < 0)
                    {
                        startAttackPos = new Vector3(0, 0, -0.14f);
                        endAttackPos = new Vector3(0, -0.3f, -0.8f);
                    }
                    // player on left
                    else if (leftOrRight.z > transform.position.z)
                    {
                        startAttackPos = new Vector3(0, 0, 0.14f);
                        endAttackPos = new Vector3(0, -0.3f, 0.8f);
                    }
                    break;
                default:
                    Debug.Log("Boyyyy, you snuffed up");
                    break;
            }

            
            startHeight = 0.7f;
            endHeight = 2;
            animSpeed = 5f * Time.deltaTime;
            currTime = 0;
            timesSwapped = 0;
            AdjustColliders();
        }
        else
        {
            Debug.Log("Charging");
            inAttackState = true;
            attackChosen = Attacks.Charge;
            animator.SetInteger("State", 1);
            attackTimer = 38f * Time.deltaTime;
            attackCollider = chargeCollider;
            attackCollider.SetActive(true);
            animSpeed = 5f * Time.deltaTime;
        }
    }

    private void AdjustColliders()
    {
        m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        switch (attackChosen)
        {
            case Attacks.Lunge:
                if (attackTimer < 0.575)
                    currTime += animSpeed;

                attackCollider.GetComponent<CapsuleCollider>().center = Vector3.Lerp(startAttackPos, endAttackPos, currTime);
                attackCollider.GetComponent<CapsuleCollider>().height = Mathf.Lerp(startHeight, endHeight, currTime);

                if (currTime > 1 && timesSwapped < 1)
                {
                    currTime = 0;
                    Vector3 tempStartPos = startAttackPos;
                    startAttackPos = endAttackPos;
                    endAttackPos = tempStartPos;
                    float tempStartHeight = startHeight;
                    startHeight = endHeight;
                    endHeight = tempStartHeight;
                    timesSwapped++;
                }
                break;
            case Attacks.Charge:
                if(Vector3.Distance(player.transform.position, transform.position) > 3f)
                {
                    attackTimer = 0;
                    Vector3 leftOrRight = player.transform.position - transform.position;
                    switch (side)
                    {
                        case 0:
                            // player on right
                            if (leftOrRight.x > 0)
                                direction = forward;
                            // player on left
                            else if (leftOrRight.x < 0)
                                direction = -forward;                                
                            break;
                        case 1:
                            // player on right
                            if (leftOrRight.z > 0)
                                direction = forward;
                            //  player on left
                            else
                                direction = -forward;
                            break;
                        case 2:
                            // player on right 
                            if (leftOrRight.x < 0)
                                direction = forward;
                            // player on left
                            else
                                direction = -forward;
                            break;
                        case 3:
                            // player on right
                            if (leftOrRight.z < 0)
                                direction = forward;
                            // player on left
                            else
                                direction = -forward;
                            //Debug.Log("Here");
                            break;
                        default:
                            Debug.Log("Boyyyy, you snuffed up");
                            break;
                    }
                    Move(false);
                }
                break;
            default:
                break;
        }
        if(attackTimer < 0.575)
            currTime += animSpeed;
    }

    void StopEverything()
    {
        animator.SetInteger("State", 0);
        inAttackState = false;
        direction = forward;
        velocity.x = 0;
    }
}
