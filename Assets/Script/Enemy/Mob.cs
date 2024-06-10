using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
