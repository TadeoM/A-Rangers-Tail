using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaselAI : Enemy
{
    enum Attacks
    {
        SwordSwing, Charge
    }
    public enum CharacterState
    {
        Idle,
        Run,
        Attack,
        Charge,
        Jump,
        Fall,
        Death
    }
    public CharacterState currentCharState;
    public GameObject floatingText;
    private Attacks attackChosen;
    public float attackTimer;
    private Animator animator;
    private bool inAttackState;
    private bool attacking;
    private bool invincible;
    private float invisTimer;
    private float lerpTime;
    public bool firstAttack;
    int iStart = 0;
    int iEnd = 1;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxSpeed = 1.5f;

        firstAttack = false;
        switch (side)
        {
            case 0:
                forward = new Vector3(1, 0, 0);
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
        creatureType = CreatureType.Weasel;
        currentCharState = CharacterState.Idle;
        inAttackState = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Health = 75;
        //attackCollider = GetComponentInChildren<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
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
            float distance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));
            if (player.GetComponent<Player_v2>().side == side && distance > 1f && distance < 6f)
            {
                Debug.Log("Weasel Running");
                ChangeState(CharacterState.Run);
                Move(false);
            }

            else if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1f && player.GetComponent<Player_v2>().side == side)
            {
                //Debug.Log("Player Side: " + player.GetComponent<Player_v2>().side + ", Centipede Side: " + side);
                Move(true);
                if (!inAttackState)
                {
                    Debug.Log("At Attack Call");
                    PerformAttack();
                }

            }
            if (player.GetComponent<Player_v2>().side != side)
            {
                Move(true);
            }
            //Debug.Log("Player Side: " + player.GetComponent<Player_v2>().side + ", Centipede Side: " + side);
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

    protected void PerformAttack()
    {

        Debug.Log("attack");
        Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        ChangeState(CharacterState.Attack);
        // needs change
        attackChosen = Attacks.SwordSwing;


    }
    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("playerweapon") && !invincible)
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
    /*
    public void ShowFloatingText()
    {
        GameObject hpText;
        if (side == 0)
        {

            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
        else if (side == 1)
        {
            Debug.Log("Here");
            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
        else if (side == 2)
        {
            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }
        else if (side == 3)
        {
            hpText = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
            hpText.GetComponent<TextMesh>().text = Health.ToString();
        }

    }
    */
    public override void TakeDamage(int dmg)
    {
        Debug.Log(Health);
        base.TakeDamage(dmg);
        invincible = true;
        invisTimer = 1.25f;
 
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
    IEnumerator AttackState()
    {

        while (currentCharState == CharacterState.Attack)
        {
            firstAttack = true;
            if(firstAttack)
            {
                animator.SetInteger("State", 2);
                yield return new WaitForSeconds(0.9f);
            }
            attackTimer = 0;
            firstAttack = false;
            if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1f)
            {
               
                inAttackState = true;
                animator.SetInteger("State", 3);
                yield return new WaitForSeconds(0.65f);
            }
            else
            {
                inAttackState = false;
                animator.SetInteger("State", 4);
                ChangeState(CharacterState.Run);
                yield return null;
            }
          
            
        }
    }
}
