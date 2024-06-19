using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloridaMan : Enemy
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Init();
        StartCoroutine(ManicEpisode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ManicEpisode()
    {

        float switchTimer = 0;
        while (true)
        {
            //Going Left
            while (true)
            {
                rb.velocity = new Vector3(-speed, 0);
                if(switchTimer >= 1.5f)
                {
                    switchTimer -= 1.5f;
                    break;
                }
                switchTimer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            //Going Left
            while (true)
            {
                rb.velocity = new Vector3(speed, 0);
                if (switchTimer >= 1.5f)
                {
                    switchTimer -= 1.5f;
                    break;
                }
                switchTimer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();

        }
    }
}
