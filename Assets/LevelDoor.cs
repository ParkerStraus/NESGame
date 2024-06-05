using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public string Level;
    public bool Active;
    public SpriteRenderer EnterArrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Active){

            EnterArrow.enabled = true;
            if(Input.GetAxisRaw("Vertical") > 0)
            {
                EnterLevel();
            }

        }
        else EnterArrow.enabled = false;
    }

    public void EnterLevel()
    {
        SceneManager.LoadScene(Level);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            Active = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Active = false;
        }
    }
}
