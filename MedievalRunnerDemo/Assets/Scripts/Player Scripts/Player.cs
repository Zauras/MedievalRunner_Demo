using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private PlayerAnimation Animator;

    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private float speed = 8f, maxVelocity = 4f, jumpForce = 10f; //movment

    public float hitDmg = 5, shotDmg = 3; //dmg
  
    [SerializeField]
    private float hp = 40f; //hp

    private Rigidbody2D playerBody;
    private bool conscious, canJump, grounded, canShoot, canHit, canMove;
    public bool isAlive;

    public enum Type { Warrior1, Warrior2, Warrior3, Archer1, Archer2, Archer3 };
    public enum LookingAt { Right, Left };
    public LookingAt Side;
    public Type PlayerType;

//  <<<<<<<<<<<<<<<<< UNITY >>>>>>>>>>>>>>>>>

    void Awake()
    {
        if (instance == null) instance = this;
        isAlive = conscious = canShoot = canHit = canMove = true;

        //SetModel();
        Side = LookingAt.Right;

        playerBody = GetComponent<Rigidbody2D>();
        Animator =  PlayerAnimation.instance;
    }

    void Update()
    {
        GameplayController.instance.PauseGame();

        if (gameObject.transform.position.y < -8f)
            PlayersDeath();

        PlayerJump();
    }

    void FixedUpdate()
    {    
        if (conscious) {
            if ( PlayerType == Type.Warrior1) PlayerAttack();
            if (PlayerType == Type.Archer1)  PlayerShoot();
            PlayerRun();
        }
    }

//  <<<<<<<<<<<<<<<<< PREPARATIONS >>>>>>>>>>>>>>>>>
    void SetModel()
    {
        if (PlayerType == Type.Warrior1)
        {
            gameObject.SetActive(true);
        }
        else if (PlayerType == Type.Warrior1)
        {
            gameObject.SetActive(true);
        }
    }

//  <<<<<<<<<<<<<<<<< MOVEMENT & INTERACTION >>>>>>>>>>>>>>>>>

    void PlayerRun()
    {
        if (canMove)
        {
            float forceX = 0f;
            float vel = Mathf.Abs(playerBody.velocity.x);
            float xAxis = Input.GetAxisRaw("Horizontal");

            if (xAxis > 0)
            {
                Side = LookingAt.Right;
                if (vel < maxVelocity)
                {
                    if (grounded) { forceX = speed; }
                    else { forceX = speed * 1.1f; }
                }

                Vector3 scale = transform.localScale;
                scale.x = 0.5f;
                transform.localScale = scale;
                Animator.WalkAnimation(); //Animation
            }
            else if (xAxis < 0)
            {
                Side = LookingAt.Left;
                if (vel < maxVelocity)
                {
                    if (grounded) { forceX = -speed; }
                    else { forceX = -speed * 1.1f; }
                }

                Vector3 temp = transform.localScale;
                temp.x = -0.5f;
                transform.localScale = temp;
                Animator.WalkAnimation(); //Animation
            }
            else
            {
                Animator.ResetIdle(); //Animation
            }
            playerBody.AddForce(new Vector2(forceX, 0));
        }
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                Animator.JumpAnimationUp(); //Animation
                StartCoroutine(WaitAndJump (0.2f, jumpForce));
                //playerBody.velocity = new Vector2(0, jumpForce); //(x, y)
                canJump = false;
            }
        }
    }
    IEnumerator WaitAndJump (float sec, float jumpForce)
    {
        yield return new WaitForSeconds(sec);
        playerBody.velocity = new Vector2(0, jumpForce); //(x, y)
    }

    void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Animator.AttackAnimation(); //Animation
            //do dmg
        }
    }

    void PlayerShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                canMove = false;
                canShoot = false;
                Animator.ShootAnimation();
                StartCoroutine(WaitShootAndReload(0.4f, 0.7f));  
            }
            //sound
        }
    }
    IEnumerator WaitShootAndReload(float waitForAnimation, float reloadTime)
    {
        yield return new WaitForSeconds(waitForAnimation);
        Vector3 arrowPosition = transform.position;
        arrowPosition.y += 0.1f;

        if (Side == LookingAt.Right)
        {
            arrowPosition.x += 0.7f;
            Vector3 temp = arrow.transform.localScale;
            temp.x = 1.5f;
            arrow.transform.localScale = temp;
        }
        else if (Side == LookingAt.Left)
        {
            arrowPosition.x -= 0.7f;
            Vector3 temp = arrow.transform.localScale;
            temp.x = -1.5f;
            arrow.transform.localScale = temp;
        }

        GameObject arrowInstance = (GameObject)Instantiate(arrow, arrowPosition, Quaternion.identity);
        arrowInstance.GetComponent<PlayerArrow>().SetPreferences(Side);

        canMove = true;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }


    void PlayersDeath()
    {
        conscious = isAlive = false;
        //sound
        Animator.DeathAnimation();
        GameplayController.instance.PlayerDied();
    }

    IEnumerator PlayerGetHit()
    {
        conscious = false;
        //sound
        Animator.GetHitAnimation();
        yield return new WaitForSeconds(0.3f);
        conscious = true;
    }

//  <<<<<<<<<<<<<<<<< COLLISIONS & TRIGGERS >>>>>>>>>>>>>>>>

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Ground")
        {
            Animator.JumpAnimationDown();  
            grounded = true;
            canJump = true;
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag == "CollectToWin")
        {
            GameplayController.instance.LevelSucceed();
        }
    }

    // <<<<<<<<<<< Getters & Setters & Others >>>>>>>>>>>>>

    public float GetHP()
    {
        return hp;
    }

    public void GetDmg(float dmg)
    {
        if (dmg >= hp || hp <= 0){
            hp = 0;
            PlayersDeath();
        }
        else {
            hp -= dmg;
            StartCoroutine(PlayerGetHit());
        }
    }

}
