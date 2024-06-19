using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrocodileBoss : Enemy
{
    public IEnumerator routine;
    public Animator anim;

    public GameObject[] ArchedProjectile;
    public Transform spanwloc;
    // Start is called before the first frame update
    void Start()
    {
        routine = EnemyBehaviour();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Provoke()
    {
        StartCoroutine(routine);
    }

    public IEnumerator EnemyBehaviour()
    {
        while (true) { 
        yield return new WaitForSeconds(Random.Range(3f,6f));
        switch(Random.Range(0,8)) {
            case 0:
            case 1:
            case 2:
            case 3:
                SlamFeet();
                break;
            case 4:
            case 5:
            case 6:
                JunkVomit();
                break;
                case 7:
                case 8:
                SuperCharge();
                break;
        }
        }
    }

    public void SlamFeet()
    {

        anim.SetTrigger("SlamFeet");
    }


    public void JunkVomit()
    {

        anim.SetTrigger("JunkVomit");
    }


    public void SuperCharge()
    {

        anim.SetTrigger("Charge");
    }

    public void VomitOBJs()
    {
        print("Vomiting");
        var pro1 = Instantiate(ArchedProjectile[Random.Range(0,ArchedProjectile.Length-1)], spanwloc.position, Quaternion.identity);
        pro1.GetComponent<ArchedProjectile>().Velocity_x = -1.5f;
        pro1.GetComponent<ArchedProjectile>().Velocity_y = 5;

        var pro2 = Instantiate(ArchedProjectile[Random.Range(0, ArchedProjectile.Length - 1)], spanwloc.position, Quaternion.identity);
        pro2.GetComponent<ArchedProjectile>().Velocity_x = -3;
        pro2.GetComponent<ArchedProjectile>().Velocity_y = 5;
        var pro3 = Instantiate(ArchedProjectile[Random.Range(0, ArchedProjectile.Length - 1)], spanwloc.position, Quaternion.identity);
        pro3.GetComponent<ArchedProjectile>().Velocity_x = -4.5f;
        pro3.GetComponent<ArchedProjectile>().Velocity_y = 5;
    }
}
