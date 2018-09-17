using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private Transform player;
    private Transform sky;
    //private float dif;

    public float minX, maxX, minY;
    

    void Start ()
    {
        player = Player.instance.transform;
        //player = GameObject.Find("PlayerArcher1").transform;
        sky = GameObject.Find("Sky").transform;
        //dif = sky.transform.position.x - transform.position.x;
    }
	
	void Update ()
    {
        CameraFollow();
        SkyFollow();
    }

    void CameraFollow()
    {
        if (player != null)
        {
            Vector3 camera = transform.position;
            camera.x = player.position.x;

            if (camera.x < minX)
                camera.x = minX;

            if (camera.x > maxX)
                camera.x = maxX;

            if (camera.y < minY)
                camera.y = minY;

            transform.position = camera;
        }
    }

    void SkyFollow()
    {
        if (player != null & sky != null)
        {
            Vector3 camera = transform.position;
            Vector3 skyVec = transform.position;
            skyVec.x = camera.x;
            sky.transform.position = new Vector3(skyVec.x, sky.transform.position.y, sky.transform.position.z);
        }
    }
}
