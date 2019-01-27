using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeAI : Enemy {

    enum Attacks
    {
        Lunge, Charge
    }
    private Attacks attackChosen; 
    private bool inAttackState;
    private int attackTimer;
    private Animator animator;

	// Use this for initialization
	public override void Start () {
        base.Start();
        creatureType = CreatureType.Bug;
        inAttackState = false;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if(!inAttackState)
        {
            PerformAttack();
        }
        if(attackTimer <= 0)
        {
            animator.SetInteger("State", 0);
            inAttackState = false;
        }
        if(attackTimer > 0)
        {
            attackTimer--;
        }
	}

    protected override void PerformAttack()
    {
        inAttackState = true;
        attackChosen = Attacks.Lunge;
        animator.SetInteger("State", 2);
        attackTimer = 60;
    }
}
