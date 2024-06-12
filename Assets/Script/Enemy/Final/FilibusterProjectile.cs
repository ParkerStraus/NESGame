using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilibusterProjectile : MonoBehaviour
{
    public int damage;
    public bool GoingRight;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GoingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            
            collision.GetComponent<Player>().Damage(damage, transform.position.x - collision.gameObject.transform.position.x);
            Object.Destroy(gameObject);
        }


        Debug.Log(collision.gameObject.name);
    }
}
