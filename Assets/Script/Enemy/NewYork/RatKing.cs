using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKing : Enemy
{
    public GameObject rat;
    public Transform spawnlocation;
    IEnumerator Summoning;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //Summon Rats
    }

    public void Provoke()
    {
        state = EnemyStates.aggro;
        Summoning = SummonLoop();
        StartCoroutine(Summoning);
    }

    IEnumerator SummonLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            var ratt = Instantiate(rat, spawnlocation);
            ratt.transform.position += new Vector3(-1, 0);
            ratt.GetComponent<Rat>().aggrorange = 99;
            yield return null;
        }
    }

    public override void Die()
    {
        StopCoroutine(Summoning);
        base.Die();
    }

}
