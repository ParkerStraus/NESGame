using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Terrortino : Boss
{

    [SerializeField] GameObject MobFab;
    [SerializeField] GameObject ProjectileFab;
    [SerializeField] private Vector2 ThrowOffset;
    [SerializeField] private Vector3[] Positions;
    [SerializeField] private float[] FlashDim;//0 is x, 1 is y

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
        //probabilities can be changed later
        if (x < 0.1)
        {
            //summons film crew to take 5 (hp off the player)
            StartCoroutine(SummonCrew());
        }
        else if (x < 0.3)
        {
            //terrortino lines up the perfect shot, stunning anything caught in the flash
            StartCoroutine(FreezeFrame());
        }
        else if (x < 0.7)
        {
            //terrortino throws a director fit and starts lobbing film equipment at the player
            StartCoroutine(ThrowAttack());
        }
        else
        {
            //terrortino leaps off the sign and lands somewhere else on it
            StartCoroutine(Move());
        }
    }

    public IEnumerator SummonCrew()
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

    public IEnumerator FreezeFrame()
    {
        //startup animation
        yield return new WaitForSeconds(0);
        float direc = Mathf.Sign(player.transform.position.x - transform.position.x);
        Vector2 p1 = new Vector2(transform.position.x+ FlashDim[0] * direc, transform.position.y + FlashDim[1] / 2);
        Vector2 p2 = new Vector2(transform.position.x, transform.position.y - FlashDim[1]/2);
        Collider2D hit = Physics2D.OverlapArea(p1,p2,1<<player.layer);
        if (hit != null)
        {
            //add player stun code here
            hit.gameObject.GetComponent<Player>();
        }
        //Transition animation
        yield return new WaitForSeconds(0);
        state = BossStates.Idle;
        cooldown = 3;
        locked = false;
        yield return null;
    }

    public IEnumerator Move()
    {
        //Animation to jump off screen
        transform.position = Positions[Mathf.RoundToInt(Random.Range(0, Positions.Length))];
        yield return new WaitForSeconds(0);
        //Jump-in animation
        yield return null;
    }

    public IEnumerator ThrowAttack()
    {
        //Note: could reuse code from Vendor
        float direc = Mathf.Sign(player.transform.position.x - transform.position.x);
        //startup animation
        yield return new WaitForSeconds(0);
        for (int i = 0; i < 3; i++)
        {
            var obj = Instantiate(ProjectileFab, transform.position + (new Vector3(ThrowOffset.x * direc, ThrowOffset.y, transform.position.z)), transform.rotation);
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


}
