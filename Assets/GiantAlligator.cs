using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GiantAlligator : Boss
{
    [SerializeField] GameObject MobFab;
    [SerializeField] GameObject ProjectileFab;
    [SerializeField] private float ChargeSpeed;
    [SerializeField] private Vector2 ThrowOffset;
    [SerializeField] private Vector3[] Positions;
    // Start is called before the first frame update
    void Start()
    {
        health = 20;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void AttackPattern()
    {
        base.AttackPattern();
        locked = true;
        float x = Random.Range(0, 1);
        if (x < 0.1)
        {
            StartCoroutine(SummonGators());
        }
        else if (x < 0.3)
        {
            StartCoroutine(JunkLaunch());
        }
        else if(x < 0.7)
        {
            StartCoroutine(GatorRush());
        }
        else
        {
            StartCoroutine(Move());
        }
    }

    public IEnumerator Move()
    {
        //Animation to jump off screen
        transform.position = Positions[Mathf.RoundToInt(Random.Range(0, Positions.Length))];
        yield return new WaitForSeconds(0);
        //Jump-in animation
        yield return null;
    }

    public IEnumerator GatorRush()
    {
        //Startup animation for gator
        yield return new WaitForSeconds(0);//animation time
        //charge animation
        float direc = Mathf.Sign(player.transform.position.x-transform.position.x);
        rb.velocity = new Vector2 (ChargeSpeed*direc, rb.velocity.y);
        yield return new WaitForSeconds(2); //adjust as needed, or add stopping at wall
        rb.velocity = Vector2.zero;
        //transition animation
        state = BossStates.Idle;
        cooldown = 3;
        locked = false;
        yield return null;
    }

    public IEnumerator SummonGators()
    {
        //Startup animation for gator
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0);
            //Maybe alter instantiate for random x offset
            Instantiate(MobFab, new Vector3(transform.position.x - 10, transform.position.y, transform.position.z), transform.rotation);
            //Add initialisation for croc mob
        }
        //Transition animation
        yield return new WaitForSeconds(0);
        state = BossStates.Idle;
        cooldown = 3;
        locked = false;
        yield return null;
    }

    public IEnumerator JunkLaunch()
    {
        //Note: could reuse code from Vendor
        float direc = Mathf.Sign(player.transform.position.x - transform.position.x);
        //startup animation
        yield return new WaitForSeconds(0);
        for(int i = 0;i < 3; i++)
        {
            var obj = Instantiate(ProjectileFab, transform.position+(new Vector3(ThrowOffset.x * direc, ThrowOffset.y,transform.position.z)),transform.rotation);
            obj.GetComponent<ArchedProjectile>().Velocity_x = direc * obj.GetComponent<ArchedProjectile>().Velocity_x;
            yield return new WaitForSeconds(0);
        }
        //Transition animation
        yield return new WaitForSeconds(0);
        state = BossStates.Idle;
        cooldown = 3;
        locked = false;
        yield return null;
    }

    public override void Die()
    {
        //Implement death animations
        base.Die();
    }
}
