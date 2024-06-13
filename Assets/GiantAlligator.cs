using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAlligator : Boss
{
    [SerializeField] Transform MobFab;
    // Start is called before the first frame update
    void Start()
    {
        health = 20;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AttackPattern()
    {
        base.AttackPattern();
        
    }

    public void GatorRush()
    {

    }

    public void SummonGators()
    {
        //Note: reuse code from Rat King
    }

    public void JunkLaunch()
    {
        //Note: could reuse code from Vendor
    }

    public override void Die()
    {
        //Implement death animations
        base.Die();
    }
}
