using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretService : Enemy
{
    public GameObject Bullet;
    public Rigidbody2D rb;
    public Transform bulletSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Init();
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
            StartCoroutine(ServiceRoutine());

        }
    }

    IEnumerator ServiceRoutine()
    {
        //aim 
        yield return new WaitForSeconds(1);

        //Shoot
        var bullet = Instantiate(Bullet, bulletSpawn.position, Quaternion.identity);
        float offset = player.position.x - transform.position.x ;
        if (offset < 0)
        {
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else
        {
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        yield return new WaitForSeconds(0.5f);
        

        //ambush
        while (true)
        {
            rb.velocity = Vector3.Normalize(new Vector3(offset, 0, 0)) * speed;
            yield return new WaitForFixedUpdate();
        }
    }
}
