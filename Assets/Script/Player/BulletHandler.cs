using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public int damage;
    public float speed;
    public float timeout;
    public bool multishot;
    //TODO:
    //Add layer mask initialisation (e.g. enemy bullet exclude enemy layer, player bullets exclude player, etc.)
    //Optimization (object pooling? raycast collision?) 

    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(Vector3.up);
        Object.Destroy(gameObject, timeout);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().Damage(damage);
        }
        if (multishot)
        {
            if(collision.gameObject.layer == 3) Destroy(gameObject);
        }
        else
        {

            Object.Destroy(gameObject);
        }

        Debug.Log(collision.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
        }
        if (multishot)
        {
            if (collision.gameObject.layer == 3) Destroy(gameObject);
        }
        else
        {

            Object.Destroy(gameObject);
        }

        Debug.Log(collision.gameObject.name);
    }
}
