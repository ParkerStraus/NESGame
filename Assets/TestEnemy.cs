using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    //UNFINISHED: trying to test chase pathfinding for enemy
    public Transform player;
    public float speed=2f;
    public float vspeed = 2f;
    public LayerMask ground;
    [SerializeField] private Rigidbody2D rb;
    private bool grounded;
    private bool jump;

    private bool obstruct;
    private bool gap;
    private bool platform;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float direc = Mathf.Sign(player.position.x - transform.position.x);
        grounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, ground);
        bool playerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1<<player.gameObject.layer);

        if (grounded)
        {
            rb.velocity = new Vector2(direc*speed, rb.velocity.y);
            obstruct = Physics2D.Raycast(transform.position, new Vector2(direc,0), 4f, ground);
            gap = !(Physics2D.Raycast(transform.position + new Vector3(direc*2,0,0), Vector2.down, 2f, ground));
            platform = Physics2D.Raycast(transform.position, Vector2.up, 3f, ground);

            if (!obstruct && gap)
            {
                jump = true;
            }
            else if (playerAbove && platform)
            {
                jump = true;
            }
            else if (obstruct)
            {
                jump = true;
            }
        }


    }

    private void FixedUpdate()
    {
        print(rb.velocity);
        if (grounded && jump)
        {
            jump = false;
            float mag = (player.position - transform.position).normalized.x * vspeed;
            rb.AddForce(new Vector2(mag,vspeed),ForceMode2D.Impulse);
        }
    }
}
