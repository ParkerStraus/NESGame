using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerEagle : Enemy
{
    public AudioClip screech;
    public Rigidbody2D rb;
    public float Velocity;
    public float VelocityDerive;
    public GameObject LazerOBJ;
    public SpriteRenderer sr;
    public float ShootCooldown;
    public float ShootCooldown_Base;
    // Start is called before the first frame update
    void Start()
    {
        TimeScale = 1;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Ensure the player GameObject is tagged 'Player'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        AggroCheck();
        ShootCooldown -= Time.deltaTime;
        
    }

    public override void AggroCheck()
    {
        float dist = transform.position.x - player.gameObject.transform.position.x;
        if(Mathf.Abs(dist) < aggrorange && state != EnemyStates.aggro)
        {
            state = EnemyStates.aggro;
            StartCoroutine(LazerRoutine());

        }
    }

    public void FixedUpdate()
    {
        rb.velocity = new Vector3(Velocity, 0, 0);
    }

    public IEnumerator LazerRoutine()
    {
        bool GoingRight = false;
        AudioSource.PlayClipAtPoint(screech, transform.position);
        while (true)
        {
            float offset = transform.position.x - player.position.x; 
            if(offset < 0)
            {
                Velocity += VelocityDerive * Time.deltaTime;
                GoingRight = true;
            }
            else
            {
                Velocity -= VelocityDerive * Time.deltaTime;
                GoingRight = false;
            }
            Velocity = Mathf.Clamp(Velocity, -speed, speed);
            sr.flipX = GoingRight;
            if (Mathf.Abs(offset) < 0.1 && ShootCooldown <= 0)
            {
                ShootCooldown = ShootCooldown_Base;
                print("Lazer Shot from eagle");
                Instantiate(LazerOBJ, transform.position, Quaternion.identity);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
