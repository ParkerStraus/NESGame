using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Health;
    public float[] range; //note: range[0] in max left x (negative), range[1] is max right x (positive)
    public float aggrorange;
    [SerializeField] private float speed;
    public Transform player;

    protected Vector3[] rangepos;
    protected Vector3  targetpos;
    protected int state = 0;
    protected float cooldown;
    //states note: 0=idle, 1=wandering, 2=aggro

    public virtual void Init()
    {
        rangepos = new Vector3[2];
        rangepos[0] = transform.position + new Vector3(range[0],0,0);
        rangepos[1] = transform.position + new Vector3(range[1],0,0);
        player = GameObject.FindWithTag("Player").transform;
    }

    private void NewWander()
    {
        float dist = Random.Range(range[0], range[1]);
        if (dist + transform.position.x < rangepos[0].x)
        {
            targetpos = rangepos[0];
        }
        else if (dist + transform.position.x > rangepos[1].x)
        {
            targetpos = rangepos[1];
        }
        else
        {
            targetpos = transform.position + new Vector3(dist, 0, 0);
        }
        state = 1;
    }

    private void MoveWander()
    {
        Vector3 newpos = Vector3.MoveTowards(transform.position, targetpos, speed * Time.deltaTime);
        transform.position = newpos;
        if (newpos == targetpos)
        {
            state = 0;
            cooldown = 2;
        }
    }

    public void Wander()
    {
        if (state == 0)
        {
            if(cooldown <= 0)
            {
                NewWander();
            }
            else
            {
                cooldown -= Time.deltaTime;
            }
        }
        else if (state == 1)
        {
            MoveWander();
        }
    }

    public void AggroCheck()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist < aggrorange)
        {
            state = 2;
        }
    }

    public void VerticalHandling()
    {

    }

    public virtual void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(1);
        }
    }
}
