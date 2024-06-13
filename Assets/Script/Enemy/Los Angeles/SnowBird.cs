using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBird : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
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
}
