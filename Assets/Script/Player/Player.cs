using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public struct PlayerData
{
    public bool GoingRight;
    public bool Walking;
    public bool OnGround;
    public float Attacking;
}

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D Rigidbody;
    public SpriteRenderer sprite;

    public float HorizontalSpeed;
    public float VerticalSpeed;

    public bool GoingLeft;

    private float H_Velocity;
    private float V_Velocity;

    [Header("Vertical Handler")]
    public float JumpSpeed;
    public float JumpTime;
    public float JumpTime_Current;
    public bool IsJumping;
    public float FallInterp;

    [SerializeField] private float RideHeight;
    [SerializeField] private float RayLength;
    [SerializeField] private float FallSpeed;
    [SerializeField] private float CoyoteTime;
    [SerializeField] private float CoyoteTime_Base;
    public bool OnGround;



    [Header("Aesthetics")]
    [SerializeField] private Sound_Player Audio;
    [SerializeField] private PlayerData m_PlayerData;
    [SerializeField] private Animator_Player anim;

    [Header("Combat")]
    [SerializeField] private Transform bulletFab;

    public int AbilitySetting;
    public bool[] AbilityFlags;
    public Sprite[] AbilitySprite;
    public SpriteRenderer AbilityIndicator;

    // Start is called before the first frame update
    void Start()
    {
        AbilityIndicator.enabled = false;

        AbilityFlags[0] = true;
        bool[] abilities = SaveSystem.LoadData().LevelCompletion;
        for (int i = 0; i < abilities.Length; i++)
        {
            print(abilities[i]);
            AbilityFlags[i + 1] = abilities[i];
        }

        m_PlayerData.GoingRight = true;
        m_PlayerData.Attacking = 0;
        m_PlayerData.Walking = false;
        m_PlayerData.OnGround = false;
        Audio = GetComponent<Sound_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalHandling();
        VerticalHandling();
        Rigidbody.velocity = new Vector2(H_Velocity, V_Velocity);
        if (Input.GetButtonDown("Select"))
        {
            Audio.AbilitySelect();
            SetAbility();
        }
        if (Input.GetButtonDown("B"))
        {
            switch (AbilitySetting)
            {
                default:
                    ShootHandle();
                    break;
                case 1:
                    RatTrampoline();
                    break;

            }
        }
        m_PlayerData.Attacking -= Time.deltaTime;
        anim.Animate(m_PlayerData);
    }

    void HorizontalHandling()
    {
        float control = Input.GetAxisRaw("Horizontal");
        m_PlayerData.Walking = true;
        if (control > 0)
        {
            m_PlayerData.GoingRight = true;
            H_Velocity = HorizontalSpeed;
        }
        else if(control < 0)
        {
            m_PlayerData.GoingRight = false;
            H_Velocity = -HorizontalSpeed;
        }
        else
        {
            H_Velocity = 0;
            m_PlayerData.Walking = false;
        }
    }

    void VerticalHandling()
    {
        float VTarget = 0;
        if (!IsJumping)
        {

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, RayLength, 1 << 3);
            if (hit)
            {
                Vector2 SnapLocation = transform.position.x * Vector2.right + (RideHeight + hit.point.y) * Vector2.up;
                transform.position = SnapLocation;
                CoyoteTime = CoyoteTime_Base;
                OnGround = true;
                VTarget = 0;

            }
            else
            {

                CoyoteTime -= Time.deltaTime;
                if (CoyoteTime <= 0)
                {
                    OnGround = false;
                    VTarget = -FallSpeed;
                }
            }
        }

        if (Input.GetButtonUp("A") && IsJumping)
        {
            JumpTime_Current = JumpTime;
        }
        if (IsJumping && JumpTime_Current >= JumpTime)
        {
            IsJumping = false;
        }

        V_Velocity = Mathf.Lerp(V_Velocity, VTarget, FallInterp*Time.deltaTime);

        if (OnGround && Input.GetButtonDown("A"))
        {
            Audio.Jump();
            print("Jumping");
            OnGround = false;
            V_Velocity = JumpSpeed;
            IsJumping = true;
            JumpTime_Current = 0;
            CoyoteTime = 0;
        }
        if (IsJumping)
        {
            V_Velocity = JumpSpeed;
            JumpTime_Current += Time.deltaTime;
        }
        m_PlayerData.OnGround = OnGround;
    }

    #region Shooting

    void ShootHandle()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        int direc = 6;
        if (m_PlayerData.GoingRight == true)
        {
            direc = 2;
        }

        if (y < 0)
        {
            if (m_PlayerData.Walking)
            {
                if (m_PlayerData.GoingRight == true) { direc = 3; }
                else if (m_PlayerData.GoingRight != true) { direc = 5; }
            }
            else { direc = 4; }
        }
        else if (y > 0)
        {
            if (m_PlayerData.GoingRight != true) { direc = 7; }
            else if (m_PlayerData.GoingRight == true) { direc = 1; }
            if(m_PlayerData.Walking==false){ direc = 0; }
        }
        direc = direc * 45;
        Quaternion angle = Quaternion.Euler(0.0f, 0.0f, -direc);
        //Note: if adding additional shot types, change this to function call
        m_PlayerData.Attacking = 0.25f;
        Audio.Shoot();
        Instantiate(bulletFab, transform.position+new Vector3(0,-0.5f,0), angle);
    }

    void RatTrampoline()
    {

    }

    #endregion

    void SetAbility()
    {
        AbilitySetting++;
        AbilitySetting = AbilitySetting % AbilityFlags.Length;

        while (AbilityFlags[AbilitySetting] == false)
        {
            AbilitySetting++;
            AbilitySetting = AbilitySetting % AbilityFlags.Length;
        }
        //Set View of ability indicator
        StopAllCoroutines();
        StartCoroutine(AbilityVisibility());
    }

    IEnumerator AbilityVisibility()
    {
        AbilityIndicator.enabled = true;
        AbilityIndicator.sprite = AbilitySprite[AbilitySetting];
        yield return new WaitForSeconds(2);
        AbilityIndicator.enabled = false;
    }
}
