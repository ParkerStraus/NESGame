using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public float damage;
    public float speed;
    public float timeout;
    //TODO:
    //Add layer mask initialisation (e.g. enemy bullet exclude enemy layer, player bullets exclude player, etc.)
    //Optimization (object pooling? raycast collision?) 

    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(Vector3.up);
        Debug.Log("Instantiated");
        Object.Destroy(gameObject, timeout);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Object.Destroy(gameObject);
        Debug.Log(collision.gameObject.name);
    }
}
