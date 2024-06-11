using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ArchedProjectile : MonoBehaviour
{
    public float Velocity_x;
    public float Velocity_y;
    public float Gravity;
    public Rigidbody2D rb;
    public SpriteRenderer Sprite;

    // Start is called before the first frame update
    void Start()
    {

        Sprite = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Velocity_y -= Gravity * Time.deltaTime;
        transform.Rotate(Vector3.forward, (360/ (-Velocity_x)) * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Velocity_x, Velocity_y);
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage(1, transform.position.x - collision.transform.position.x);
        }
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
