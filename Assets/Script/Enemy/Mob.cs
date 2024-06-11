using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mob : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        Init();
        stun = false;
        knockback = false;
    }

    // Update is called once per frame
    //NOTE: as this is an archetype class, update should be moved to enemy instances
    void Update()
    {
        if (TimeScale > 0) return;
        if (state == EnemyStates.idle || state == EnemyStates.wander)
        {
            //Passive Loop
            Wander();
            AggroCheck();
        }
        else
        {
            //Aggro loop
            //chase code
            //or should enemy not move for attacks?
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }
            else
            {
                //attack code
            }
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
