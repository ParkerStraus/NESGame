using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalHandling();
        VerticalHandling();
        Rigidbody.velocity = new Vector2(H_Velocity, V_Velocity);
    }

    void HorizontalHandling()
    {
        float control = Input.GetAxisRaw("Horizontal");
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

    }
}
