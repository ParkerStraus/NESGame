using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public struct PlayerData
{
    public bool GoingRight;
    public bool Walking;
    public bool OnGround;
    public float Attacking;
    public float Damaged;
    public float Sunshine;
}

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D Rigidbody;
    public SpriteRenderer sprite;

    public float HorizontalSpeed;
    public float VerticalSpeed;

    public bool GoingLeft;

    [SerializeField] private float H_Velocity;
    [SerializeField] private float V_Velocity;

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
    public int Health;
    public int Health_Max;
    public float DamageCooldown;
    public float DamageCooldown_Time;

    [SerializeField] private Transform bulletFab;

    public int AbilitySetting;
    public bool[] AbilityFlags;
    public float[] AbilityRecharge;
    public float RechargeTime;
    public IEnumerator RechargeReadyRoutine;
    public Sprite[] AbilitySprite;
    public SpriteRenderer AbilityIndicator;

    [SerializeField] private GameObject FreezeBeamOBJ;

    // Start is called before the first frame update
    void Start()
    {
        AbilityIndicator.enabled = false;

        AbilityFlags[0] = true;
        bool[] abilities = SaveSystem.LoadData().LevelCompletion;
        AbilityRecharge = new float[AbilityFlags.Length];
        AbilityRecharge[0] = 0;
        for (int i = 0; i < abilities.Length; i++)
        {
            //print(abilities[i]);
            AbilityFlags[i + 1] = abilities[i];
            AbilityRecharge[i + 1] = -1;
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
                    if (AbilityRecharge[0] <= 0) LAAbility(); 
                    break;
                case 2:
                    if (AbilityRecharge[1] <= 0) FloridaAbility();
                    break;
                case 3:
                    if (AbilityRecharge[2] <= 0) NYCAbility();
                    break;
                case 4:
                    if (AbilityRecharge[3] <= 0) TexasAbility();
                    break;
                case 5:
                    if (AbilityRecharge[4] <= 0) MinneAbility();
                    break;

            }
        }
        m_PlayerData.Attacking -= Time.deltaTime;
        anim.Animate(m_PlayerData);
        DamageCooldown -= Time.deltaTime;
        AbilityRecharging();
    }

    #region Positioning

    public void SetLocation(string Location)
    {
        string[] coords = Location.Split(',');
        gameObject.transform.position = new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
    }

    void HorizontalHandling()
    {
        if (m_PlayerData.Damaged > 0)
        {
            m_PlayerData.Damaged -= Time.deltaTime;
            return;
        }
        if (!GameHandler.CanThePlayerMove)
        {
            H_Velocity = 0;
            m_PlayerData.Walking = false;
            return;
        }
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

        if (Input.GetButtonUp("A") && IsJumping && GameHandler.CanThePlayerMove)
        {
            JumpTime_Current = JumpTime;
        }
        if (IsJumping && JumpTime_Current >= JumpTime)
        {
            IsJumping = false;
        }

        V_Velocity = Mathf.Lerp(V_Velocity, VTarget, FallInterp*Time.deltaTime);

        if (OnGround && Input.GetButtonDown("A") && GameHandler.CanThePlayerMove)
        {
            Audio.Jump();
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

    #endregion

    #region Shooting

    void AbilityRecharging()
    {
        for(int i = 0; i < AbilityRecharge.Length; i++)
        {
            float TempValue = AbilityRecharge[i];
            AbilityRecharge[i] -= Time.deltaTime;
            if (AbilityRecharge[i] < 0 && TempValue >= 0)
            {
                if(RechargeReadyRoutine != null)StopCoroutine(RechargeReadyRoutine);
                RechargeReadyRoutine = RechargeReady(i);
                StartCoroutine(RechargeReadyRoutine);
            }
        }
        
        m_PlayerData.Sunshine -= Time.deltaTime;
    }

    IEnumerator RechargeReady(int i)
    {
        AbilityIndicator.enabled = true;
        AbilityIndicator.sprite = AbilitySprite[i + 1];
        yield return new WaitForSeconds(1.5f);
        AbilityIndicator.enabled = false;
    }

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


    void LAAbility()
    {
        //Freeze enemy
        AbilityRecharge[0] = 8;

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
            if (m_PlayerData.Walking == false) { direc = 0; }
        }
        direc = direc * 45;
        Quaternion angle = Quaternion.Euler(0.0f, 0.0f, -direc);
        //Note: if adding additional shot types, change this to function call
        Audio.Shoot();
        Instantiate(FreezeBeamOBJ, transform.position + new Vector3(0, -0.5f, 0), angle);
    }
    void FloridaAbility()
    {
        AbilityRecharge[1] = 30;
        m_PlayerData.Sunshine = 7;
    }
    void NYCAbility()
    {
        AbilityRecharge[2] = 4;
        OnGround = false;
        V_Velocity = JumpSpeed * 1.5f;
        IsJumping = true;
        StartCoroutine(RatTramp());
    }
    IEnumerator RatTramp()
    {
        float time = 0.25f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            V_Velocity = JumpSpeed * 1.5f;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    void TexasAbility()
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
            if (m_PlayerData.Walking == false) { direc = 0; }
        }
        direc = direc * 45;
        Quaternion angle = Quaternion.Euler(0.0f, 0.0f, -direc);
        //Note: if adding additional shot types, change this to function call
        var BUL = Instantiate(bulletFab, transform.position + new Vector3(0, -0.5f, 0), angle);
        BUL.GetComponent<BulletHandler>().damage = 5;
        BUL.GetComponent<BulletHandler>().multishot = true;
        BUL.GetComponent<BulletHandler>().speed = 30.75f;
        AbilityRecharge[3] = 5;
    }
    void MinneAbility()
    {
        AbilityRecharge[4] = 30;
    }

    #endregion

    #region General Info
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
        FindAnyObjectByType<GameHandler>().AbilityIndicatorSet();
    }


    public void Damage(int damage, float Offset)
    {
        if (m_PlayerData.Sunshine > 0) return;
        if(DamageCooldown < 0)
        {
            DamageCooldown = DamageCooldown_Time;
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
            m_PlayerData.Damaged = 0.8f;
            if(Offset < 0)
            {
                H_Velocity = 2;
            }
            else
            {
                H_Velocity = -2;
            }
        }
    }

    public void Heal(int health)
    {
        Health += health;
        if(Health >= Health_Max)
        {
            Health = Health_Max;
        }
    }

    public void Die()
    {

        GameObject.Destroy(gameObject);
    }


    #endregion
}
