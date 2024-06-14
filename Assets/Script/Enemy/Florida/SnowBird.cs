using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBird : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        this.animator.Animate("Moving");
        Init();
        StartCoroutine(SnowBirdRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SnowBirdRoutine()
    {
        bool GoingRight = true;
        while (true)
        {
            if(GoingRight)
            {
                transform.position += new Vector3(speed, 0) * Time.deltaTime;
                if(transform.position.x > range[1])
                {
                    GoingRight = !GoingRight;
                }
            }
            else
            {
                transform.position -= new Vector3(speed, 0) * Time.deltaTime;
                if (transform.position.x < range[0])
                {
                    GoingRight = !GoingRight;
                }
            }
        }
    }

    public override void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (TimeScale == 0) return;
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(1, transform.position.x - collision.transform.position.x);
            StartCoroutine(SBAnimate());
        }
    }

    IEnumerator SBAnimate()
    {
        this.animator.Animate("Melee");
        yield return new WaitForSeconds(0.5f);
        this.animator.Animate("Moving");
    }
}
