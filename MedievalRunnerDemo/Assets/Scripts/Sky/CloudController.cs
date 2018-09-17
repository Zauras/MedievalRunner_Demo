using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{

    [SerializeField]
    private float speed1 = 0.1f,
                  speed2 = 0.2f;

    [SerializeField]
    private GameObject nearCloud1, nearCloud2, nearCloud3,
                       midCloud1,  midCloud2,  midCloud3,
                       farCloud1,  farCloud2,  farCloud3,

                       nearCloud3_1, nearCloud1_2, nearCloud2_2,
                       midCloud1_1,  midCloud2_2,  midCloud3_1,
                       farCloud1_2,  farCloud2_2,  farCloud3_1;

    [SerializeField]
    private Transform leftBound, rightBound;

    private List<GameObject> cloudsToLeft = new List<GameObject>();
    private List<GameObject> cloudsToLeft1 = new List<GameObject>();  //use speed2
    private List<GameObject> cloudsToRight = new List<GameObject>(); 
    private List<GameObject> cloudsToRight1 = new List<GameObject>(); //use speed2

//  <<<<<<<<<<<<<<<<< UNITY >>>>>>>>>>>>>>>>>

    void Start()
    {   //The Middle:                   //The Right Side:                 //The Left Side:
        cloudsToRight.Add(nearCloud1);                                    cloudsToRight.Add(nearCloud1_2);
        cloudsToRight1.Add(nearCloud2);                                   cloudsToRight1.Add(nearCloud2_2);
        cloudsToLeft.Add(nearCloud3);   cloudsToLeft.Add(nearCloud3_1);   

        cloudsToLeft.Add(midCloud1);    cloudsToLeft.Add(midCloud1_1);    
        cloudsToRight.Add(midCloud2);                                     cloudsToRight.Add(midCloud2_2);
        cloudsToLeft1.Add(midCloud3);   cloudsToLeft1.Add(midCloud3_1);   

        cloudsToRight.Add(farCloud1);                                     cloudsToRight.Add(farCloud1_2);
        cloudsToRight1.Add(farCloud2);                                    cloudsToRight1.Add(farCloud2_2);
        cloudsToLeft.Add(farCloud3);    cloudsToLeft.Add(farCloud3_1);    
    }

    void Update()
    {
        Collect();
    }

    void FixedUpdate ()
    {
        Movement();
    }


 //  <<<<<<<<<<<<<<<<< MOVEMENT >>>>>>>>>>>>>>>>>

    void Movement()
    {
        foreach (GameObject cloud in cloudsToRight)
        {
            cloud.transform.Translate(Vector3.right * Time.deltaTime * speed1, Camera.main.transform);
        }

        foreach (GameObject cloud in cloudsToRight1)
        {
            cloud.transform.Translate(Vector3.right * Time.deltaTime * speed2, Camera.main.transform);
        }

        foreach (GameObject cloud in cloudsToLeft)
        {
            cloud.transform.Translate(Vector3.left * Time.deltaTime * speed1, Camera.main.transform);
        }

        foreach (GameObject cloud in cloudsToLeft1)
        {
            cloud.transform.Translate(Vector3.left * Time.deltaTime * speed2, Camera.main.transform);
        }

    }

//<<<<<<<<<<<<<<<<INTERACTION >>>>>>>>>>>>>>>>>

    void Collect()
    {
        Vector3 temp = leftBound.position;

        foreach (GameObject cloud in cloudsToRight)
        {
            if (cloud.transform.position.x >= rightBound.position.x)
            {
                temp = leftBound.position;
                temp.y = cloud.transform.position.y;
                cloud.transform.position = temp;

            }
        }
        foreach (GameObject cloud in cloudsToRight1)
        {
            if (cloud.transform.position.x >= rightBound.position.x)
            {
                temp = leftBound.position;
                temp.y = cloud.transform.position.y;
                cloud.transform.position = temp;

            }
        }
        foreach (GameObject cloud in cloudsToLeft)
        {
            if (cloud.transform.position.x <= leftBound.position.x)
            {
                temp = rightBound.position;
                temp.y = cloud.transform.position.y;
                cloud.transform.position = temp;

            }
        }
        foreach (GameObject cloud in cloudsToLeft1)
        {
            if (cloud.transform.position.x <= leftBound.position.x)
            {
                temp = rightBound.position;
                temp.y = cloud.transform.position.y;
                cloud.transform.position = temp;

            }
        }
    }

    void Spawn(GameObject cloud, List<GameObject> List)
    {
        if (List == cloudsToLeft)
        {
            cloud = Instantiate(cloud, rightBound.position, transform.rotation);
            cloudsToLeft.Add(cloud);
        }
        else if (List == cloudsToLeft1)
        {
            cloud = Instantiate(cloud, rightBound.position, transform.rotation);
            cloudsToLeft.Add(cloud);
        }
        else if (List == cloudsToRight)
        {
            cloud = Instantiate(cloud, leftBound.position, transform.rotation);
            cloudsToLeft.Add(cloud);
        }
        else if (List == cloudsToRight1)
        {
            cloud = Instantiate(cloud, leftBound.position, transform.rotation);
            cloudsToLeft.Add(cloud);
        }
    }

} //
