
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public enum Phases
    {
        game,
        boss,
        end
    }
    public Phases currentPhase;
    public int Level;
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
    public GameObject[] HealthBarPics;
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
        if(currentPhase == Phases.boss && Boss.Health <= 0)
        {
            currentPhase = Phases.end;
            GameComplete();
        }
        UISet();
    }

    void UISet()
    {
        for(int i = 0; i < HealthBarPics.Length; i++)
        {
            if(player.Health >= i)
            {
                HealthBarPics[i].SetActive(true);
            }
            else
            {
                HealthBarPics[i].SetActive(false);
            }
        }
        Health.text = "Health:";
        BossHealth.text = "Boss: " + Boss.Health;
    }

    public void StartBoss()
    {
        currentPhase = Phases.boss;
        BossHealth.enabled = true;
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
                AbilityText.text = "Take 5";
                break;
            case 2:
                //Florida
                AbilityText.text = "Sunshine Shield";
                break;
            case 3:
                //New York
                AbilityText.text = "Rat Trampoline";
                break;
            case 4:
                //Texas
                AbilityText.text = "Cool Hat";
                break;
            case 5:
                //Minnesota
                AbilityText.text = "Purple Rain";
                break;
        }
        yield return new WaitForSeconds(2);
        AbilityImage.enabled = false;
        AbilityText.enabled = false;
        if(InBoss)BossHealth.enabled = true;
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
        StartCoroutine(GameCompleteRoutine());
    }

    IEnumerator GameCompleteRoutine()
    {
        print("Boss Defeated");
        SaveSystem.SaveLevelProgress(Level, true);
        yield return new WaitForSeconds(3);
        CanThePlayerMove = false;
        print("Put cool fanfare here");
        yield return new WaitForSeconds(3);
        print("Yell new ability");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Tutorial");
        CanThePlayerMove = true;
    }
}
