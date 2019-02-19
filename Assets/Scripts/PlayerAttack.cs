using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    //Attacks
    float attkTimer;
    float coolDown = 0.75f;
    public int dmg;
    float startSpecial;
    bool special;
    float specialTimer;
    float specialCD = 3.0f;
    bool attacking;
    string weaponType;
    Player_v2 player;
    public Collider swordHitBox;
    //Player Animation
    private Animator playerAnimator;
    private SpriteRenderer playerRenderer;
    Vector3 swordInitialPos;
    Vector3 swordDist;
    float travelSpeed = 1.0f;
    private int timesSwapped;
    private float currTime;
    // Use this for initialization
    void Awake ()
    {
        player = gameObject.GetComponent<Player_v2>();
        playerAnimator = gameObject.GetComponent<Animator>();
        swordHitBox.enabled = false;
        attacking = false;
        dmg = 20;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0) && !attacking && !special)
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
        if(Input.GetMouseButtonDown(1) && !special)
        {
            FlurryThrow();
            startSpecial = Time.time;
            special = true;
            specialTimer = specialCD;

        }

        if(special)
        {
            if(specialCD>0)
            {
                specialCD -= Time.deltaTime;
            }
            else
            {
                special = false;
                
                swordHitBox.enabled = false;

                startSpecial = 0;
            }
        }
       
    }

    void FlurryThrow()
    {

        dmg = 25;
        swordInitialPos = this.gameObject.transform.GetChild(0).transform.localPosition;

        swordDist = player.transform.position;
        swordDist.x += 1.5f;
        float distCovered = (Time.time - startSpecial)*travelSpeed;
        Debug.Log(swordDist);
        float fracJourney = distCovered / 1.5f;

        swordHitBox.transform.position = Vector3.Lerp(this.gameObject.transform.GetChild(0).transform.localPosition, swordDist, fracJourney);
        swordHitBox.enabled = true;
        if (currTime > 1 && timesSwapped < 1)
        {
            currTime = 0;
            Vector3 tempStartPos = swordInitialPos;
            swordInitialPos = swordDist;
            swordDist = tempStartPos;
            timesSwapped++;
        }
        if (special==false)
        {
            swordHitBox.transform.localPosition = swordInitialPos;
        }
       
    }

    void SetDamageType(string type)
    {
        if(type=="bug" && weaponType == "piercing")
        {

        }
    }

}
