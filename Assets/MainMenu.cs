using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject cursor;
    public int CursorPos = 0;
    public Vector2[] CursorPositioning;

    public GameObject Text_NewGame;
    public GameObject Text_Continue;
    public bool SaveFound;
    // Start is called before the first frame update
    void Start()
    {
        if (SaveSystem.CheckData())
        {
            Text_Continue.SetActive(true);
            SaveFound = true;
        }
        else
        {
            Text_Continue.SetActive(false);
            SaveFound = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        cursor.transform.position = Vector2.Lerp(cursor.transform.position, CursorPositioning[CursorPos], 8f * Time.deltaTime);
        if(SaveFound)
        {
            if(Input.GetAxisRaw("Vertical") < 0)
            {
                CursorPos = 1;
            }
            else
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                CursorPos = 0;
            }
        }

        if (Input.GetButtonDown("Start"))
        {
            switch(CursorPos)
            {
                case 0:
                    SaveSystem.InitData();
                    SceneManager.LoadScene("Tutorial");
                    break;
                case 1:
                    SceneManager.LoadScene("Tutorial");
                    break;
                default:
                    SceneManager.LoadScene("Tutorial");
                    break;
            }
        }

    }
}
