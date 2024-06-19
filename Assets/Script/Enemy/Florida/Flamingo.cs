using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamingo : Enemy
{
    public float FlightHeight;
    public float HeightSpeed;
    public Rigidbody2D Rigidbody;

    public BoxCollider2D boxCol;
    public Vector2 CollideIdle_Offset;
    public Vector2 CollideIdle_Size;

    public Vector2 CollideFly_Offset;
    public Vector2 CollideFly_Size;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.offset = CollideIdle_Offset;
        boxCol.size = CollideIdle_Size;

    Init();
    }

    // Update is called once per frame
    void Update()
    {
        AggroCheck();
    }

    public override void OnAggro()
    {
        StartCoroutine(Fly());
    }

    public IEnumerator Fly()
    {
        animator.Animate("Moving");
        boxCol.size = CollideFly_Size;
        boxCol.offset = CollideFly_Offset;
        while (true)
        {
            if(transform.position.y >= FlightHeight)
            {
                transform.position = new Vector2(transform.position.x, FlightHeight);
                break;
            }
            transform.position += new Vector3(0, HeightSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        //Fly Forward
        while (true)
        {

            Rigidbody.velocity = new Vector2(-speed, 0);
            yield return new WaitForEndOfFrame();   
        }
    }
}
