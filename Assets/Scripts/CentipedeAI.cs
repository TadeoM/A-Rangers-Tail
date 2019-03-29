using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeAI : Enemy {

    enum Attacks
    {
        Lunge, Charge
    }
    public enum CharacterState
    {
        Idle,
        Run,
        Lunge,
        Charge,
        Jump,
        Fall,
        Death
    }
    public CharacterState currentCharState;
    public GameObject floatingText;
    private Attacks attackChosen;
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
    private bool attacking;
    private bool invincible;
    private float invisTimer;
    private float lerpTime;
    int iStart = 0;
    int iEnd = 1;
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

        Debug.Log(gameObject.transform.forward);
        creatureType = CreatureType.Bug;
        currentCharState = CharacterState.Idle;
        inAttackState = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Health = 100;
        //attackCollider = GetComponentInChildren<CapsuleCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        if(player != null)
        {
            
            Vector3 leftOrRight = player.transform.position - transform.position;
            CheckPlayer();
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
            
            if (player.GetComponent<Player_v2>().side == side && Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) > 3f)
            {
                Debug.Log(Vector3.Distance(player.transform.position, transform.position));
                ChangeState(CharacterState.Run);
                Move(false);
            }
            else if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 3f && player.GetComponent<Player_v2>().side == side)
            {
                Debug.Log("Player Side: " + player.GetComponent<Player_v2>().side + ", Centipede Side: " + side);
                Move(true);
                if (!inAttackState)
                {
                    Debug.Log("At Attack Call");
                        PerformLunge();
                }

            }
            if(player.GetComponent<Player_v2>().side !=side)
            {
                Move(true);
            }
            Debug.Log("Player Side: " + player.GetComponent<Player_v2>().side + ", Centipede Side: " + side);
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

    protected override void PerformLunge()
    {

        Debug.Log("attack");
        Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        ChangeState(CharacterState.Lunge);
        // needs change
        inAttackState = true;
            attackChosen = Attacks.Lunge;
             
     
        
        /*
        else
        {
            Debug.Log("Charging");
            Debug.Log(Vector3.Distance(player.transform.position, transform.position));
            inAttackState = true;
            attackChosen = Attacks.Charge;
            animator.SetInteger("State", 1);
            
            attackCollider = chargeCollider;
            attackCollider.SetActive(true);
            ChangeState(CharacterState.Lunge);
        }
        */
    }
    /*
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
    */
    private void OnTriggerEnter(Collider col)
    {
       
        if (col.gameObject.CompareTag("playerweapon")&&!invincible)
        {
            TakeDamage(25);
            player.GetComponent<Player_v2>().combo += 1;
        }
        
        
    }
    void StopEverything()
    {
        animator.SetInteger("State", 0);
        inAttackState = false;
        direction = forward;
        velocity.x = 0;
    }
    public void ShowFloatingText()
    {
        GameObject hpText;
            if (side==0)
        {

            hpText = Instantiate(floatingText, transform.position, Quaternion.identity , transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
            else if (side==1)
        {
            Debug.Log("Here");
            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
            else if(side==2)
        {
            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
            else if(side==3)
        {
            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
        
    }
    public override void TakeDamage(int dmg)
    {
        Debug.Log(Health);
        base.TakeDamage(dmg);
        invincible = true;
        invisTimer = 1.25f;
        if (floatingText && Health>0)
            ShowFloatingText();
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void ChangeState(CharacterState newState)
    {
        currentCharState = newState;
        StartCoroutine(newState.ToString() + "State");
    }
    IEnumerator IdleState()
    {
        while (currentCharState == CharacterState.Idle)
        {
            animator.SetInteger("State", 0);
            yield return null;
        }
    }

    IEnumerator RunState()
    {
        while (currentCharState == CharacterState.Run)
        {
            animator.SetInteger("State", 1);
            yield return null;
        }
    }
    IEnumerator LungeState()
    {
   
        while (currentCharState == CharacterState.Lunge)
        {
            attackTimer = 0;
            animator.SetInteger("State", 2);
            yield return new WaitForSeconds(1.25f);
            inAttackState = false;
            ChangeState(CharacterState.Run);
        }
    }
    
}
