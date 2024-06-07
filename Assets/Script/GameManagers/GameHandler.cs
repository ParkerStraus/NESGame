
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static int Lives;
    public static bool CanThePlayerMove;
    public Player player;
    public bool PlayerAlive = true;
    public MusicHandler musicHandler;
    public AudioClip BossLoop;
    public AudioClip BossIntro;
    public bool InBoss;
    public Enemy Boss;

    [Header("UI")]
    public TMP_Text Health;
    public TMP_Text AbilityText;
    public Image AbilityImage;
    public Sprite[] AbilitySprites;
    public TMP_Text BossHealth;
    public IEnumerator UI;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        CanThePlayerMove = true;
        musicHandler = GameObject.Find("Music").GetComponent<MusicHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.Health <= 0 && PlayerAlive == true)
        {
            PlayerAlive = false;
            CanThePlayerMove = false;
            StartCoroutine(OnPlayerDeath());
        }
        UISet();
    }

    void UISet()
    {
        Health.text = "Health: " + player.Health;
        BossHealth.text = "Boss: " + Boss.Health;
    }

    public void StartBoss()
    {
        InBoss = true;
        musicHandler.QueueNewSong(BossIntro, BossLoop);
    }

    public void AbilityIndicatorSet()
    {   if(UI != null)StopCoroutine(UI);
        UI = AbilityVisibility(player.AbilitySetting);
        StartCoroutine(UI);
    }

    IEnumerator AbilityVisibility(int AbilitySetting)
    {
        BossHealth.enabled = false;
        AbilityText.enabled = true;
        AbilityImage.enabled = true;
        AbilityImage.sprite = AbilitySprites[AbilitySetting];
        switch (AbilitySetting)
        {
            case 0:
                //Normal
                AbilityText.text = "ProtoBlaster";
                break;
            case 1:
                //LA
                AbilityText.text = "LA Ability";
                break;
            case 2:
                //NYC
                AbilityText.text = "NYC Ability";
                break;
            case 3:
                //Florida
                AbilityText.text = "Flor Ability";
                break;
            case 4:
                //Texas
                AbilityText.text = "Texas Ability";
                break;
            case 5:
                //Minnesota
                AbilityText.text = "Minn Ability";
                break;
        }
        yield return new WaitForSeconds(2);
        AbilityImage.enabled = false;
        AbilityText.enabled = false;
        BossHealth.enabled = true;
    }

    IEnumerator OnPlayerDeath()
    {
        Lives--;
        musicHandler.StopMusic();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GameComplete()
    {

    }
}
