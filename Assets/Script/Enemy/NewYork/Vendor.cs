using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vendor : Enemy
{
    public GameObject ThrownObject;
    public float ThrowTime;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(Throw());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Throw()
    {
        while (true) {
        yield return new WaitForSeconds(ThrowTime);
        var obj = Instantiate(ThrownObject);

        if (player.transform.position.x < transform.position.x)
        {
            obj.GetComponent<ArchedProjectile>().Velocity_x = -Mathf.Abs(obj.GetComponent<ArchedProjectile>().Velocity_x);
        }
        else
        {
            obj.GetComponent<ArchedProjectile>().Velocity_x = Mathf.Abs(obj.GetComponent<ArchedProjectile>().Velocity_x);
        }
        
            yield return null;
        }
    }

    public override void Die()
    {
        StopAllCoroutines();

        base.Die();
    }
}
