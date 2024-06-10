using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        AggroCheck();
        if (state == EnemyStates.aggro)
        {
            
        }
    }

    public override void AggroCheck()
    {
        float dist = transform.position.x - player.position.x;
        if (Mathf.Abs(dist) < aggrorange)
        {
            state = EnemyStates.aggro;
        }
    }
}

