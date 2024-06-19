using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MrAmericaBoss : Enemy
{
    public IEnumerator routine;
    public Animator anim;

    public GameObject Projectile;
    public GameObject Eagle;
    public GameObject[] Hand;
    public Transform spanwloc;

    public float HandLocFactor_L;
    public Vector3 HandLocBase_L;
    public float HandLocFactor_R;
    public Vector3 HandLocBase_R;

    public Vector3 playerPos = Vector3.zero;
    public bool posLock = false;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        routine = EnemyBehaviour();
        HandLocBase_L = Hand[0].transform.position;
        HandLocBase_R = Hand[1].transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(!posLock)
        {

            playerPos = Vector3.Lerp(playerPos, player.transform.position, 4f * Time.deltaTime);
        }
        Hand[0].transform.position = new Vector3(Mathf.Lerp(HandLocBase_L.x, playerPos.x, HandLocFactor_L), Hand[0].transform.position.y);
        Hand[1].transform.position = new Vector3(Mathf.Lerp(HandLocBase_R.x, playerPos.x, HandLocFactor_R), Hand[1].transform.position.y);
    }

    public void Provoke()
    {
        StartCoroutine(routine);
    }

    public IEnumerator EnemyBehaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 6f));
            switch (Random.Range(0, 8))
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    FistFall();
                    break;
                case 6:
                case 7:
                case 8:
                    EagleCall();
                    break;
            }
        }
    }

    public void EagleCall()
    {
        Instantiate(Eagle, spanwloc);
    }

    public void FistFall()
    {
        ;
        if(Random.Range(0, 1) == 0)
        {
            anim.SetTrigger("Fist_L");
        }
        else
        {
            anim.SetTrigger("Fist_R");
        }
    }

    public void LazerShoot()
    {

    }


}
