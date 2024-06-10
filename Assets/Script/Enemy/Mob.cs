using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackInfo
{
    public float range;
    public float damage;
    public bool stun;
    public float stuntime;
    public bool knockback;
    public float knockbackforce;
}

public class Mob : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyStates.idle || state == EnemyStates.wander)
        {
            //Passive Loop
            Wander();
            AggroCheck();
        }
        else
        {
            //Aggro loop
            
        }
    }

    void Melee()
    {
        //switch to attack animation
        //on 'hit' frame of animation (or each frame to be complicated), raycast on player layer
        //raycast length will need to be manually set, see 'range' in attackinfo
        //on hit, damage according to attack info
    }
}
