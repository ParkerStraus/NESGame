using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : Enemy
{
    public GameObject poop;
    public bool Provoked = false;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        AggroCheck();
        if (state == EnemyStates.aggro && Provoked == false)
        {
            Provoked = true;
            StartCoroutine(PoopRoutine());
        }
    }

    IEnumerator PoopRoutine()
    {
        float time = 0;
        while (time <= 3)
        {
            time += Time.deltaTime * TimeScale;
            //Adjust position to intercept player
            PositionToPlayer();
            yield return new WaitForEndOfFrame();
        }
        //Fly up away
        Poop();
        while (time <= 6)
        {
            time += Time.deltaTime * TimeScale;
            this.transform.position += Vector3.up * 4 * Time.deltaTime * TimeScale;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    void PositionToPlayer()
    {
        this.transform.position = Vector3.Lerp(transform.position, new Vector2(player.gameObject.transform.position.x, transform.position.y), speed * Time.deltaTime * TimeScale);
    }

    void Poop()
    {
        var pooop = Instantiate(poop, this.transform);
        pooop.transform.position -= Vector3.up;
        pooop.transform.parent = null;
    }
}
