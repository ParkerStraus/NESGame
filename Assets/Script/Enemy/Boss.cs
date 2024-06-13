using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    protected enum BossStates{
        Idle,
        Damage,
        Die,
        Attack
        }
    protected BossStates state = BossStates.Idle;
    protected float cooldown;
    protected bool locked=false;
    [SerializeField] protected GameObject player;

    public void Damage(int dmg)
    {
        locked = true;
        health -= dmg;
        if (health < 0)
        {
            state = BossStates.Die;
            StopAllCoroutines();
        }
        locked = false;
        return;
    }

    private IEnumerator TestAttack()
    {
        //Attack code goes here
        state= BossStates.Idle;
        cooldown = 3;
        locked = false;
        yield return null;
    }

    public virtual void AttackPattern()
    {
        //Note:
        //override in implemented classes
        //should contain attack decision making, starting of coroutines, locking the state machine, etc.
        
    }

    //Boss death animation
    //Instantiate death effect
    //Destroy object (either delayed coroutine or animation event)
    public virtual void Die()
    {
        Object.Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Basic working:
        //State machine. On idle state, wait out cooldown. Otherwise, set to attack state.
        //In attack, choose action using AttackPattern(), lock and execute, then set to idle, unlock, and set cooldown.
        //lock prevents state changes during an action. cooldown allows a period of idle time before next action
        //all attacks, movements, etc. will be actions of the boss, rather than continuously updating executions. This leads to the classic "boss attack patterns" type of gameplay.
        if (locked) return;
        else
        {
            switch (state)
            {
                case BossStates.Idle:
                    if (cooldown > 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        state = BossStates.Attack;
                    }
                    break;
                case BossStates.Attack:
                    locked = true;
                    AttackPattern();
                    break;
                case BossStates.Die:
                    Die();
                    break;
                default:
                    break;
            }
        }
    }

}
