using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Politician : Enemy
{
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
        
    }
}
