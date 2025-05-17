using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Transform m_transform;
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Animator m_animator;

    // ANIMATOR IDS
    private int idSpeed;
    private int idIsGrounded;
    private int idIsWallDetected;
    
    [Header("Move Settings")] 
    [SerializeField] private float speed;
    private int direction = 1;
    
    [Header("Jump Settings")] 
    [SerializeField] private int extraJump;
    [SerializeField] private int counterExtraJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canDoubleJump;
   
    [Header("Ground Settings")]
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    private RaycastHit2D lFootRay;
    private RaycastHit2D rFootRay;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Wall Settings")]
    [SerializeField] private float wallDistanceRay;
    [SerializeField] private bool isWallDetected;
    [SerializeField] private bool canWallSlide;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpDuration;
    

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
       // m_transform = GetComponent<Transform>();
        m_gatherInput = GetComponent<GatherInput>();
        m_animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("IsGrounded");
        idIsWallDetected = Animator.StringToHash("IsWallDetected");
        rFoot = GameObject.Find("R_Foot").GetComponent<Transform>();
        lFoot = GameObject.Find("L_Foot").GetComponent<Transform>();
        counterExtraJump = extraJump;
    }

    void Update()
    {
        SetAnimatorValue();
    }

    private void SetAnimatorValue()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
        m_animator.SetBool(idIsWallDetected, isWallDetected);
    }

    void FixedUpdate()
    {
        CheckCollision();
        Move(); 
        JumpForce();
    }
    
    private void CheckCollision()
    {
        HandleGround();
        HandleWalls();
        HandleWallSlide();
    }


    private void HandleGround()
    {
        lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        if (lFootRay || rFootRay)
        {
            isGrounded = true;
            counterExtraJump = extraJump;
            canDoubleJump = false;
        }
        else
        {
            isGrounded = false;
        }
    }
    
    private void HandleWalls()
    {
        isWallDetected = Physics2D.Raycast(m_transform.position, Vector2.right * direction, wallDistanceRay, groundLayer);
    }
    
    private void HandleWallSlide()
    {
        canWallSlide = isWallDetected;
        if (!canWallSlide) return;
        // canDoubleJump = true; // lo tengo deshabilitado porque en mi caso no era necesario
        wallSlideSpeed = m_gatherInput.Value.y < 0 ? 1 : 0.5f;
        m_rigidbody2D.linearVelocity = new Vector2(m_rigidbody2D.linearVelocityX, m_rigidbody2D.linearVelocityY * wallSlideSpeed);
        
    }

    private void Move()
    {
        if (isWallDetected && !isGrounded) return;
        if (isWallJumping) return;
        Flip();
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.Value.x, m_rigidbody2D.linearVelocity.y );
    }

    private void Flip()
    {
        if (m_gatherInput.Value.x * direction < 0)
        {
            HandleDirection();
        }
    }

    private void HandleDirection()
    {
        m_transform.localScale = new Vector3(-m_transform.localScale.x, m_transform.localScale.y, m_transform.localScale.z);
        direction *= -1;
    }

    private void JumpForce()
    {
        if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
            {
               m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.Value.x, jumpForce);
               canDoubleJump = true;
            }
            else if (isWallDetected)
            {
                WallJump();
            }
            else if (counterExtraJump > 0 && canDoubleJump)
            {
                DoubleJump();
            }
        } 
        m_gatherInput.IsJumping =  false;
    }

    private void WallJump()
    { 
        m_rigidbody2D.linearVelocity = new Vector2(wallJumpForce.x * -direction, wallJumpForce.y);
        HandleDirection();
        StartCoroutine(WallJumpCoroutine());
    }

    IEnumerator WallJumpCoroutine()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }
    private void DoubleJump()
    {
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.Value.x, jumpForce); 
        counterExtraJump--;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + (wallDistanceRay * direction), m_transform.position.y));
    }
}
