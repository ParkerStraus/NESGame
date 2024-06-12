using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class Animator_Player : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sprite;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int ShootWalk = Animator.StringToHash("Shoot Walk");
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    public Sound_Player sound;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Animate(PlayerData data)
    {
        if(data.Damaged > 0f)
        {
            anim.CrossFade(Damaged, 0f);
        }
        if (!data.OnGround)
        {
            anim.CrossFade(Jump, 0f);
        }
        else if (data.Walking)
        {
            if (data.Attacking > 0)
            {
                anim.CrossFade(ShootWalk, 0f);
            }
            else anim.CrossFade(Walk, 0f);
        }
        else
        {
            if (data.Attacking > 0)
            {
                anim.CrossFade(Shoot, 0f);
            }
            else
            {
                anim.CrossFade(Idle, 0f);
            }
        }


        sprite.flipX = !data.GoingRight;






    }

    public void FootStep()
    {
        sound.FootStep();
    }
}
