using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    private enum BossStates{
        Idle,
        Damage,
        Die,
        Attack1
        }
    private BossStates state = BossStates.Idle;
    private float cooldown;
    private bool locked=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Damage(int dmg)
    {
        locked = true;
        health -= dmg;
        if (health < 0)
        {
            state = BossStates.Die;
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

    // Update is called once per frame
    void Update()
    {
        //Basic working:
        //checks boss state. if idle, wait out cooldown. otherwise, set new random attack. if an attack, lock and execute the attack, then set to idle, unlock, and set cooldown.
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
                        //replace values with range for attack states
                        state = (BossStates) Random.Range(0, 0);
                    }
                    break;
                case BossStates.Attack1:
                    locked = true;
                    StartCoroutine(TestAttack());
                    break;
                    break;
                case BossStates.Die:
                    //Boss death animation
                    //Instantiate death effect
                    //Destroy object (either delayed coroutine or animation event)
                    break;
                default:
                    break;
            }
        }
    }

}
