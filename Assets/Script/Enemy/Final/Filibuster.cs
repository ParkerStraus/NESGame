using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filibuster : Enemy
{
    IEnumerator Activity;
    public GameObject bubbleOBJ;
    public Transform bubbleSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Activity = FilibusterAct();
    }

    // Update is called once per frame
    void Update()
    {
        AggroCheck();

    }

    public override void AggroCheck()
    {
        float dist = transform.position.x - player.gameObject.transform.position.x;
        if (Mathf.Abs(dist) < aggrorange && state != EnemyStates.aggro)
        {
            state = EnemyStates.aggro;

            StartCoroutine(Activity);
        }
    }

    public IEnumerator FilibusterAct()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(1.0f);
            var bubble = Instantiate(bubbleOBJ, bubbleSpawn.position, Quaternion.identity);
            float offset = player.position.x - transform.position.x;
            if (offset > 0)
            {
                bubble.GetComponent<FilibusterProjectile>().GoingRight = true;
            }
            else
            {
                bubble.GetComponent<FilibusterProjectile>().GoingRight = false;
            }

            //Send Bubble of shit
            animator.Animate("Melee");
            yield return new WaitForSeconds(0.75f);
            animator.Animate("Idle");

        }
    }

    public override void Die()
    {
        StopCoroutine(Activity);
        base.Die();
    }
}
