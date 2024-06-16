using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Politician : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        TimeScale = 1;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Ensure the player GameObject is tagged 'Player'.");
        }
    }

    public override IEnumerator OnAttack()
    {
        yield return base.OnAttack();
        animator.Animate("Melee");
        yield return new WaitForSeconds(0.40f);
        animator.Animate("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
