using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeAI : Enemy {

    enum Attacks
    {
        Lunge, Charge
    }
    private float currTime;
    private float animSpeed;
    private Vector3 startAttackPos;
    private Vector3 endAttackPos;
    private float startHeight;
    private float endHeight;
    public CapsuleCollider attackCollider;
    private Attacks attackChosen; 
    private bool inAttackState;
    public float attackTimer;
    private Animator animator;
    private int timesSwapped;

	// Use this for initialization
	public override void Start () {
        base.Start();
        creatureType = CreatureType.Bug;
        inAttackState = false;
        animator = GetComponent<Animator>();
        //attackCollider = GetComponentInChildren<CapsuleCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		if(!inAttackState)
        {
            PerformAttack();
        }
        else
        {
            AdjustColliders();
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }
        if(attackTimer <= 0)
        {
            animator.SetInteger("State", 0);
            inAttackState = false;
            attackTimer = 0;
        }
        
	}

    protected override void PerformAttack()
    {

        inAttackState = true;
        attackChosen = Attacks.Lunge;
        animator.SetInteger("State", 2);
        attackTimer = 38f * Time.deltaTime;
        attackCollider.gameObject.SetActive(true);
        startAttackPos = new Vector3(-0.14f, 0, 0);
        endAttackPos = new Vector3(-0.8f, -0.3f, 0);
        startHeight = 0.7f;
        endHeight = 2;
        animSpeed = 5f * Time.deltaTime ;
        Debug.Log(animSpeed);
        currTime = 0;
        timesSwapped = 0;
        AdjustColliders();
    }

    private void AdjustColliders()
    {
        if(attackTimer < 0.6)
            currTime += animSpeed;

        attackCollider.center = Vector3.Lerp(startAttackPos, endAttackPos, currTime);
        attackCollider.height = Mathf.Lerp(startHeight, endHeight, currTime);
        if(currTime > 1 && timesSwapped < 1)
        {
            currTime = 0;
            Vector3 tempStartPos = startAttackPos;
            startAttackPos = endAttackPos;
            endAttackPos = tempStartPos;
            float tempStartHeight = startHeight;
            startHeight = endHeight;
            endHeight = tempStartHeight;
        }
    }
}
