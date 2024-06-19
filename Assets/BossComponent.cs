using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossComponent : Enemy
{
    public Enemy BossObj;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Damage(int damage)
    {
        BossObj.Damage(damage);
    }
}
