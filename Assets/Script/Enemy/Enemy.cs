using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Enemy : MonoBehaviour
{
    public int Health;
    public float[] range; //note: range[0] in max left x (negative), range[1] is max right x (positive)
    public float aggrorange;
    [SerializeField] protected float speed;
    public Transform player;
    public float attackrange;

    public float TimeScale;
    public bool Freezeable;
    public EnemyAnimator animator;

    protected Vector3[] rangepos;
    protected Vector3  targetpos;
    public enum EnemyStates
    {
        idle,
        wander,
        aggro
    };
    [SerializeField] protected EnemyStates state = EnemyStates.idle;
    protected float cooldown;
    protected bool stun = false;
    protected bool knockback = false;
    protected Vector2 kbforce = Vector2.zero;

    public virtual void Init()
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
        rangepos = new Vector3[2];
        if (range.Length != 2)
        {
            return;
        }
        
            rangepos[0] = transform.position + new Vector3(range[0], 0, 0);
            rangepos[1] = transform.position + new Vector3(range[1], 0, 0);
        
    }

    #region Passive Loop Function
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
        state = EnemyStates.wander;
    }


    private void MoveWander()
    {
        Vector3 newpos = Vector3.MoveTowards(transform.position, targetpos, speed * Time.deltaTime*TimeScale);
        transform.position = newpos;
        if (newpos == targetpos)
        {
            state = EnemyStates.idle;
            cooldown = 2;
        }
    }

    public void Wander()
    {
        if (state == EnemyStates.idle)
        {
            if(cooldown <= 0)
            {
                NewWander();
            }
            else
            {
                cooldown -= Time.deltaTime * TimeScale;
            }
        }
        else if (state == EnemyStates.wander)
        {
            MoveWander();
        }
    }

    public virtual void AggroCheck()
    {
        float dist = transform.position.x - player.position.x;
        if(Mathf.Abs(dist) < aggrorange)
        {
            state = EnemyStates.aggro;
        }
    }

    #endregion

    public virtual void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    #region Attack Functions

    public void MeleeCheck()
    {
        float direc = Mathf.Sign(player.position.x - transform.position.x);
        bool inRange = Physics2D.Raycast(transform.position, Vector2.right * direc, attackrange, 1 << player.gameObject.layer);
        if (inRange)
        {
            //attack animation
            //animation has func call event
        }
    }

    public void MeleeAttack()
    {
        float direc = Mathf.Sign(player.position.x-transform.position.x);
        Collider2D check = Physics2D.OverlapBox(transform.position + new Vector3(attackrange * direc, 0, 0), new Vector2(2, 2), 0, 1 << player.gameObject.layer);
        if(check){
            BaseAttack(check.gameObject.GetComponent<Player>(),transform.position.x-check.transform.position.x);
        }
        cooldown = 2;
    }

    public void RangedCheck()
    {
        float direc = Mathf.Sign(player.position.x - transform.position.x);
        bool inRange = Physics2D.Raycast(transform.position, Vector2.right * direc, 999, 1 << player.gameObject.layer);
        if (inRange)
        {
            //attack animation
            //with even call
        }
    }

    public void RangedAttack()
    {
        float direc = Mathf.Sign(player.position.x - transform.position.x);
        //instantiate bullet here
        //set velocity of bullet, with direction
        cooldown = 2;
    }

    public void SummonAttack()
    {
        //Go to animation state, instantiate multiple mobs
    }

    public virtual void BaseAttack(Player player, float offset)
    {
        player.Damage(1, offset);
        if (stun)
        {
            //stun player function
        }
        if (knockback)
        {
            //knockback player
        }
    }

    #endregion

    public void Freeze()
    {
        if (Freezeable) StartCoroutine(FreezeRoutine());
    }

    IEnumerator FreezeRoutine()
    {
        TimeScale = 0;
        GetComponent<SpriteRenderer>().color = Color.cyan;
        yield return new WaitForSeconds(6);
        TimeScale = 1;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (TimeScale == 0) return;
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(1, transform.position.x - collision.transform.position.x) ;
            StartCoroutine(OnAttack());
        }
    }

    public void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        if (TimeScale == 0) return;
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(1, transform.position.x - collision.transform.position.x);
            StartCoroutine(OnAttack());
        }
    }

    public virtual IEnumerator OnAttack()
    {
        yield return null;
    }
}
