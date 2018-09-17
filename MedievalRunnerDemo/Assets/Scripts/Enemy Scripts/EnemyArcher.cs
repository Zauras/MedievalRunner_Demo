using System.Collections;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    [SerializeField]
    private float hp = 15f, speed = 8f, dmg = 6f;

    private Player player;

    [SerializeField]
    private Transform attackZoneUp, attackZoneDown, endPos;

    [SerializeField]
    private GameObject bounceLeft, bounceRight;

    [SerializeField]
    private GameObject arrow;

    private bool detectionUp, detectionDown, canMove, canShoot, alive = true;

    private Rigidbody2D enemyBody;
    private Animator animator;

    public enum LookingAt { Left, Right };
    public LookingAt Side;

 //  <<<<<<<<<<<<<<<<< UNITY >>>>>>>>>>>>>>>>>
    void Awake()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Side = LookingAt.Left;
        canShoot = canMove = true;
    }

    void Start()
    {
        player = Player.instance;
    }

    void FixedUpdate()
    {
        if (alive)
        {
            if (player.isAlive) DetectPlayer();
            Move();
        }
        //make bounce not with trigger but with linecast
    }

//  <<<<<<<<<<<<<<<<< MOVEMENT >>>>>>>>>>>>>>
    void Move()
    {
        if (canMove)
        {
            animator.SetBool("Walk", true);
            enemyBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
        }
        else { animator.SetBool("Walk", false); }
    }

    void ChangeDirection()
    {
        Vector3 temp = transform.localScale;

        if (temp.x == 0.5f)
        {
            temp.x = -0.5f;
            Side = LookingAt.Left;
        }
        else
        {
            temp.x = 0.5f;
            Side = LookingAt.Right;
        }
        transform.localScale = temp;
    }

//  <<<<<<<<<<<<<<<<< INTERACTION WITH PLAYER >>>>>>>>>>>>>>>>>>>>>>>

    void DetectPlayer()
    {

        detectionUp = Physics2D.Linecast(attackZoneUp.position, endPos.position, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(attackZoneUp.position, endPos.position, Color.green);

        detectionDown = Physics2D.Linecast(attackZoneDown.position, endPos.position, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(attackZoneDown.position, endPos.position, Color.red);

        if (detectionUp || detectionDown)
        {
            canMove = false;
            StartCoroutine(Alert(4f));
            if (canShoot)
            {
                canShoot = false;
                animator.Play("Archer2_Attack1");
                StartCoroutine(AnimatedShot(0.4f, 0.7f));
            }
        }
    }
    IEnumerator Alert(float alertTime)
    {
        if (!canMove){
            yield return new WaitForSeconds(alertTime);
            canMove = true;
        }
    }
    IEnumerator AnimatedShot(float waitForAnimation, float reloadTime)
    {
        yield return new WaitForSeconds(waitForAnimation);
        Vector2 arrowPosition = transform.position;
        arrowPosition.y += 0.1f;

        if (Side == LookingAt.Right)
        {
            arrowPosition.x += 0.7f;
            Vector2 temp = arrow.transform.localScale;
            temp.x = 1.5f;
            arrow.transform.localScale = temp;
        }
        else if (Side == LookingAt.Left)
        {
            arrowPosition.x -= 0.7f;
            Vector2 temp = arrow.transform.localScale;
            temp.x = -1.5f;
            arrow.transform.localScale = temp;
        }
        GameObject arrowInstance = (GameObject)Instantiate(arrow, arrowPosition, Quaternion.identity);
        arrowInstance.GetComponent<EnemyArrow>().SetPreferences(Side, dmg, player);
            
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

    void Death()
    {
        //sound
        alive = false;
        animator.Play("Archer2_Death");
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
        else{
            hp -= damage;
        }
    }

//  <<<<<<<<<<<<<<<<< COLLISIONS & TRIGGERS >>>>>>>>>>>>>>>>

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "PlayerArrow")
        {
            animator.Play("Archer2_Hit");
            GetDmg(player.shotDmg);
            //play sound
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject == bounceLeft || target.gameObject == bounceRight )//& canMove)
            ChangeDirection();

        if (target.gameObject.tag == "Arrow"){
            animator.Play("Archer2_Hit");
            //play sound
        }

    }

}

