using System.Collections;
using UnityEngine;

public class EnemyWarrior : MonoBehaviour
{
    [SerializeField]
    private float hp = 15f, dmg = 5f, speed = 8f;

    private Player player;

    [SerializeField]
    private GameObject bounceLeft, bounceRight;
    [SerializeField]
    private Transform attackZone, endPos, attackZoneBack, endPosBack;

    private Rigidbody2D enemyBody;
    private Animator animator;

    private bool detection, detectionBack;
    private bool canHit, canDmg, canWalk, alive;
    private bool alert, attack;
 
//  <<<<<<<<<<<<<<<<< UNITY >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    void Awake()
    {
        canHit = canDmg = canWalk = alive = true;
        alert = attack = false;

        enemyBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = Player.instance;
    }

    void FixedUpdate()
    {
        if (alive){
            if (player.isAlive) DetectPlayer();
            Move();
        }
    }

//  <<<<<<<<<<<<<<<<< MOVEMENT >>>>>>>>>>>>>>>>>>>>>>>>>>>>

    void Move()
    {   
        if (canWalk == true & alert == false)
        {
            animator.Play("Warrior2_Walk");
            animator.SetBool("Walk", true);
            enemyBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
        }
        else { animator.SetBool("Walk", false); }
    }

    void ChangeDirection()
    {
            Vector3 temp = transform.localScale;
            if (temp.x == 0.5f) { temp.x = -0.5f;} //to Left
            else        { temp.x = 0.5f; } //to Right
            transform.localScale = temp;   //set new Scale
    }

//  <<<<<<<<<<<<<<<<< INTERACTION WITH PLAYER >>>>>>>>>>>>>>

    void DetectPlayer()
    {
        detection = Physics2D.Linecast(attackZone.position, endPos.position, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(attackZone.position, endPos.position, Color.red);

        if (detection & canHit){
            alert = true;
            StartCoroutine(AnimatedAttack(0.6f)); // 0.6f unitl attack animmation reach player
        }
        else { alert = false; }

        detectionBack = Physics2D.Linecast(attackZoneBack.position, endPosBack.position, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(attackZoneBack.position, endPosBack.position, Color.green);

        if (detectionBack) ChangeDirection();
    }
    IEnumerator AnimatedAttack(float sec)
    {
        canHit = canWalk = false; //Preparing
        animator.Play("Warrior2_Attack1");
        //play sound
        yield return new WaitForSeconds(sec); // 0.6f unitl attack animmation reach player

        attack = true; // do dmg;
        yield return new WaitForSeconds(0.15f);  // if <0.15 animation will broke

        alert = attack = false;
        canHit = canWalk = true;
    }

    private void Death()
    {
        //sound
        alive = false;
        animator.Play("Warrior2_Death");
        Destroy(enemyBody); //set off arrow Rigidbody2D
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 8.0f);
        //drop Item, give score and etc
    }
    private void GetDmg(float damage)
    {
        if (damage >= hp || hp <= 0){
            hp = 0;
            Death();
        }
        else {
            hp -= damage;
        }
    }

    IEnumerator DoingDamage(float sec)
    {
        canDmg = false;
        player.GetDmg(dmg);
        yield return new WaitForSeconds(sec);
        canDmg = true;
    }

//  <<<<<<<<<<<<<<<<< COLLISIONS & TRIGGERS >>>>>>>>>>>>>>>>

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.gameObject.tag == "Player" & attack == true)
        {
            if (canDmg) StartCoroutine(DoingDamage(0.15f)); // if <0.15 animation will broke
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject == bounceLeft || target.gameObject == bounceRight)
            ChangeDirection();
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "PlayerArrow")
        {
            animator.Play("Warrior2_Hit");
            GetDmg(player.shotDmg);
            //play sound
        }
        // !!kaip player warrior!!
    }

}
