using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Moving");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Melee = Animator.StringToHash("Melee");
    private static readonly int Damaged = Animator.StringToHash("Damaged"); //should damage not be animation

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Animate(string state)
    {
        switch (state)
        {
            case "Idle":
                animator.CrossFade(Idle, 0);
                break;
            case "Moving":
                animator.CrossFade(Walk, 0);
                break;
            case "Melee":
                animator.CrossFade(Melee, 0);
                break;
            case "Ranged":
                animator.CrossFade(Shoot, 0);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
