using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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

    [Header("Sound")]
    public AudioSource AS;

    [Serializable]
    public struct PlayerData
    {
        public bool Walking;
        public bool OnGround;
        public bool Attacking;
    }

    [SerializeField] private PlayerData m_PlayerData;
    [SerializeField] private Transform bulletFab;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerData.Attacking = false;
        m_PlayerData.Walking = false;
        m_PlayerData.OnGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalHandling();
        VerticalHandling();
        Rigidbody.velocity = new Vector2(H_Velocity, V_Velocity);
        if (Input.GetButtonDown("Select"))
        {
            //Add weapon swap here
        }
        if (Input.GetButtonDown("B"))
        {
            ShootHandle();
        }
    }

    void HorizontalHandling()
    {
        float control = Input.GetAxisRaw("Horizontal");
        m_PlayerData.Walking = true;
        if (control > 0)
        {
            H_Velocity = HorizontalSpeed;
        }
        else if(control < 0)
        {
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

    void ShootHandle()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        int direc = 2;
        if(x < 0)
        {
            direc = 6;
        }

        if (y < 0)
        {
            if (x < 0) { direc = 5; }
            else if (x > 0) { direc = 3; }
            else { direc = 4; }
        }
        else if (y > 0)
        {
            if (x < 0) { direc = 7; }
            else if (x > 0) { direc = 1; }
            else { direc = 0; }
        }
        direc = direc * 45;
        Quaternion angle = Quaternion.Euler(0.0f, 0.0f, -direc);
        //Note: if adding additional shot types, change this to function call
        Instantiate(bulletFab, transform.position, angle);
    }
}
