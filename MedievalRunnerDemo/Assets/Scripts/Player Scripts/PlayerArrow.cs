using System.Collections;
using UnityEngine;

public class PlayerArrow : MonoBehaviour {

    public PlayerArrow instance;
    private float maxVelocity = 8f;
    private float speed = 8f;
    private Rigidbody2D arrowBody;
    private bool ToTheRight = true, landed = false;

    private Player.LookingAt Side;
    private Quaternion arrowRotation;

//  <<<<<<<<<<<<<<<<< UNITY >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    void Start()
    {
        arrowBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        arrowRotation = gameObject.transform.rotation;
    }

    void FixedUpdate()
    {
        if (!landed) MoveArrow();
    }

//  <<<<<<<<<<<<<<<<< MOVEMENT >>>>>>>>>>>>>>>>>>>>>>>>>>>>

    void MoveArrow()
    {
        speed = Random.Range(speed - 2f, speed);
        float forceX = 0f;
        float forceY = 0f;
        float vel = Mathf.Abs(arrowBody.velocity.x);
        bool setted = false;

        if (setted == false)
        {
            if (Side == Player.LookingAt.Left) ToTheRight = false;
            setted = true;
        }

        if (ToTheRight) // jei priesais Player
        {
            if (vel < maxVelocity) //max pagreitis
            {
                forceX = speed;  //perdavimas
                forceY = speed / 7;  //perdavimas
            }
            if (arrowBody.rotation >= -90f) //rotate arrow during time
                arrowBody.MoveRotation(arrowBody.rotation - (40f) * Time.fixedDeltaTime);
        }
        else // jei priesais Player
        {
            if (vel < maxVelocity)//max pagreitis
            { 
                forceX = -speed;  //perdavimas 
                forceY = speed / 7;
            }
            if (arrowBody.rotation <= 90f) //rotate arrow during time
                arrowBody.MoveRotation(arrowBody.rotation + (40f) * Time.fixedDeltaTime);
        }
        arrowBody.AddForce(new Vector2(forceX, forceY));
    }

//  <<<<<<<<<<<<<<<<< COLLISIONS & TRIGGERS >>>>>>>>>>>>>>>>

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        if (target.gameObject.tag != "Player")
        {
            landed = true;
            Destroy(arrowBody); //set off arrow Rigidbody2D
            Destroy(GetComponent<BoxCollider2D>());
            
            gameObject.transform.rotation = arrowRotation;
            transform.parent = target.gameObject.transform; //other here being the target
            Destroy(gameObject, 8.0f);
        }
    }

//  <<<<<<<<<<<<<<<<< GETTERS & SETTERS >>>>>>>>>>>>>>>>

    public void SetPreferences(Player.LookingAt Side)
    {
        this.Side = Side;
    }

}
