using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //PLAYER COMPONENTS
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;

    [Header("Move and Jump Settings")] 
    [SerializeField] private float speed;
    private int direction = 1;
    [SerializeField] private int extraJump;
    [SerializeField] private int counterExtraJump;
    [SerializeField] private float jumpForce;
    private int idSpeed;
    
    [Header("Ground Settings")]
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
    private int idIsGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();
        m_gatherInput = GetComponent<GatherInput>();
        m_animator = GetComponent<Animator>();
        idSpeed = Animator.StringToHash("Speed");
        idIsGrounded = Animator.StringToHash("IsGrounded");
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
    }

    void FixedUpdate()
    {
        Move(); 
        JumpForce();
        CheckGround();
    }
    
    private void Move()
    {
        Flip();
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, m_rigidbody2D.linearVelocity.y );
    }

    private void Flip()
    {
        if (m_gatherInput.ValueX * direction < 0)
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, m_transform.localScale.y, m_transform.localScale.z);
            direction *= -1;
        }
    }
    private void JumpForce()
    {
        if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
            {
               m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce); 
            }

            if (counterExtraJump > 0)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce); 
                counterExtraJump--;
            }
        } 
        m_gatherInput.IsJumping =  false;
    }
    private void CheckGround()
    {
        RaycastHit2D lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D RFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        if (lFootRay || RFootRay)
        {
            isGrounded = true;
            counterExtraJump = extraJump;
        }
        else
        {
            isGrounded = false;
        }
    }
}
