using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    //Attacks
    float attkTimer;
    float coolDown = 0.75f;
   
    bool attacking;

    public Collider2D swordHitBox;
    //Player Animation
    private Animator playerAnimator;
    private SpriteRenderer playerRenderer;
    // Use this for initialization
    void Awake ()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        swordHitBox.enabled = false;
        attacking = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0) && !attacking)
        {
            attacking = true;
            attkTimer = coolDown;
            swordHitBox.enabled = true;
        }

        if(attacking)
        {
            if(attkTimer>0)
            {
                attkTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                
                swordHitBox.enabled = false;
            }
        }
        playerAnimator.SetBool("isPlayerAttacking", attacking);
       
    }

}
