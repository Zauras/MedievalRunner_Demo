using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public static PlayerAnimation instance;
    private Animator anim;

    public bool jumping = false;
    Player.Type PlayerType;


    void Awake()
    {
        if (instance == null) { instance = this; }
        //MakeSingleton();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        PlayerType = Player.instance.PlayerType;
    }

    void MakeSingleton()
    {
        if (instance != null){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }

        public void ResetIdle()
    {
        anim.SetBool("Walk", false);
    }

    public void WalkAnimation()
    {
        anim.SetBool("Walk", true);
    }

    public void JumpAnimationUp()
    {
        if      (PlayerType == Player.Type.Archer1)  { anim.Play("Archer1_JumpUp");}
        else if (PlayerType == Player.Type.Warrior1) { anim.Play("Warrior1_JumpUp");}
        jumping = true;
    }

    public void JumpAnimationDown()
    {
        if (jumping == true)
        {
            if      (PlayerType == Player.Type.Archer1)  { anim.Play("Archer1_JumpDown");}
            else if (PlayerType == Player.Type.Warrior1) { anim.Play("Warrior1_JumpDown");}

            jumping = false;  
        }
    }

    public void ShootAnimation()
    {
        if (PlayerType == Player.Type.Archer1) { anim.Play("Archer1_Attack1");}
    }

    public void AttackAnimation()
    {
        if (PlayerType == Player.Type.Warrior1) { anim.Play("Warrior1_Attack1"); }
    }

    public void GetHitAnimation()
    {
        if      (PlayerType == Player.Type.Archer1)  { anim.Play("Archer1_Hit");}
        else if (PlayerType == Player.Type.Warrior1) { anim.Play("Warrior1_Hit");}
    }

    public void DeathAnimation()
    {
        if      (PlayerType == Player.Type.Archer1)  { anim.Play("Archer1_Death");}
        else if (PlayerType == Player.Type.Warrior1) { anim.Play("Warrior1_Death");}
    }

}
