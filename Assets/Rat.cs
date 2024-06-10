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
        float dist = transform.position.x - player.gameObject.transform.position.x;
        if (Mathf.Abs(dist) < aggrorange)
        {
            state = EnemyStates.aggro;
            StartCoroutine(Scurry());
        }
    }

    public IEnumerator Scurry()
    {
        while (true)
        {
            //Go Forward 
            if(transform.position.x > player.gameObject.transform.position.x)
            {

                transform.position -= new Vector3(speed*Time.deltaTime, 0, 0);
            }
            else
            {

                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            //Check if collided with Player
            yield return new WaitForEndOfFrame();
        }
    }
}

