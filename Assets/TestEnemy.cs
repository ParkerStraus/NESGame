using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    //UNFINISHED: trying to test chase pathfinding for enemy
    public Transform player;
    public float speed=2f;

    private Rigidbody2D rb;
    private bool grounded;
    private bool jump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1<<3);
        float direc = Mathf.Sign(player.position.x - transform.position.x);
        bool above = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1<<player.gameObject.layer);
        if (grounded)
        {

        }
    }
}
