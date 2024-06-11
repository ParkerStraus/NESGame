using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vendor : Enemy
{
    public GameObject ThrownObject;
    public Transform ThrowPoint;
    public Sprite ThrowSprite;
    public Sprite[] GuySprite;
    public SpriteRenderer GuySpriteRen;
    public IEnumerator throwing;
    public float ThrowTime;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        throwing = Throw();
        StartCoroutine(throwing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Throw()
    {
        while (true) {
            if (TimeScale != 0)
            {
                yield return new WaitForSeconds(ThrowTime / TimeScale);
                
                var obj = Instantiate(ThrownObject);
                obj.transform.position = ThrowPoint.transform.position;
                GuySpriteRen.sprite = GuySprite[1];
                obj.GetComponent<SpriteRenderer>().sprite = ThrowSprite;

                if (player.transform.position.x < transform.position.x)
                {
                    obj.GetComponent<ArchedProjectile>().Velocity_x = -Mathf.Abs(obj.GetComponent<ArchedProjectile>().Velocity_x);
                }
                else
                {
                    obj.GetComponent<ArchedProjectile>().Velocity_x = Mathf.Abs(obj.GetComponent<ArchedProjectile>().Velocity_x);
                }
                yield return new WaitForSeconds(0.75f / TimeScale);
                GuySpriteRen.sprite = GuySprite[0];
            }

            yield return null;
        }
    }

    public override void Die()
    {
        StopCoroutine(throwing);

        base.Die();
    }
}
