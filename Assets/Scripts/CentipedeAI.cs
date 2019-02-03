using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeAI : Enemy {

    enum Attacks
    {
        Lunge, Charge
    }
    private Attacks attackChosen;
    public GameObject player;
    public CapsuleCollider attackCollider;
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
        else
        {
            AdjustColliders();
        }
        if(attackTimer <= 0)
        {
            animator.SetInteger("State", 0);
            inAttackState = false;
        }

        attackTimer -= Time.deltaTime;
    }

    protected override void PerformAttack()
    {
        Debug.Log(Vector3.Distance(player.transform.position, position));
        if (Vector3.Distance(player.transform.position, position) <= 4.5f)
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
            animSpeed = 5f * Time.deltaTime;
            currTime = 0;
            timesSwapped = 0;
            AdjustColliders();
        }
        else
        {
            inAttackState = true;
            attackChosen = Attacks.Charge;
        }
    }

    private void AdjustColliders()
    {
        if(attackTimer < 0.575)
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
            timesSwapped++;
        }
    }
}
