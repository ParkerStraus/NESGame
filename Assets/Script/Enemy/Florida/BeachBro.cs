using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBro : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override IEnumerator OnAttack()
    {
        yield return base.OnAttack();

        float offset = player.position.x - transform.position.x;
        GetComponent<SpriteRenderer>().flipX = (offset > 0) ? true : false;
        animator.Animate("Melee");
        yield return new WaitForSeconds(0.40f);
        animator.Animate("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
