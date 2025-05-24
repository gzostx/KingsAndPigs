using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("m_transform")]
    [Header("Components")] 
    [SerializeField] private Transform mTransform;
    private Rigidbody2D _mRigidbody2D;
    private GatherInput _mGatherInput;
    private Animator _mAnimator;

    // ANIMATOR IDS
    private int _idSpeed;
    private int _idIsGrounded;
    private int _idIsWallDetected;
    private int _idKnockBack;
    
    [Header("Move Settings")] 
    [SerializeField] private float speed;
    [SerializeField] private bool canMove;
    [SerializeField] private float moveDelay;
    private int _direction = 1;
    
    
    [Header("Jump Settings")] 
    [SerializeField] private int extraJump;
    [SerializeField] private int counterExtraJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canDoubleJump;
   
    [Header("Ground Settings")]
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    private RaycastHit2D _lFootRay;
    private RaycastHit2D _rFootRay;
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
    
    [Header("Knock Settings")]
    [SerializeField] private bool isKnocked;
   // [SerializeField] private bool canBeKnocked;
    [SerializeField] private Vector2 knockedPower;
    [SerializeField] private float knockedDuration;
    
    [Header("Death VFX")]
    [SerializeField] private GameObject deathVFXPrefab;

    private void Awake()
    {
        _mRigidbody2D = GetComponent<Rigidbody2D>();
       // m_transform = GetComponent<Transform>();
        _mGatherInput = GetComponent<GatherInput>();
        _mAnimator = GetComponent<Animator>();
        canMove = false;
        StartCoroutine(CanMoveRoutine());
    }

    private IEnumerator CanMoveRoutine()
    {
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _idSpeed = Animator.StringToHash("speed");
        _idIsGrounded = Animator.StringToHash("IsGrounded");
        _idIsWallDetected = Animator.StringToHash("IsWallDetected");
        _idKnockBack = Animator.StringToHash("KnockBack");
        counterExtraJump = extraJump;
    }

    private void SetAnimatorValue()
    {
        _mAnimator.SetFloat(_idSpeed, Mathf.Abs(_mRigidbody2D.linearVelocityX));
        _mAnimator.SetBool(_idIsGrounded, isGrounded);
        _mAnimator.SetBool(_idIsWallDetected, isWallDetected);
    }
    
    void Update()
    {
        SetAnimatorValue();
    }
    void FixedUpdate()
    {
        if (!canMove) return;
        if (isKnocked) return;
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
        _lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        _rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        if (_lFootRay || _rFootRay)
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
        isWallDetected = Physics2D.Raycast(mTransform.position, Vector2.right * _direction, wallDistanceRay, groundLayer);
    }
    
    private void HandleWallSlide()
    {
        canWallSlide = isWallDetected;
        if (!canWallSlide) return;
        // canDoubleJump = true; // lo tengo deshabilitado porque en mi caso no era necesario
        wallSlideSpeed = _mGatherInput.Value.y < 0 ? 1 : 0.5f;
        _mRigidbody2D.linearVelocity = new Vector2(_mRigidbody2D.linearVelocityX, _mRigidbody2D.linearVelocityY * wallSlideSpeed);
        
    }

    private void Move()
    {
        if (isWallDetected && !isGrounded) return;
        if (isWallJumping) return;
        Flip();
        _mRigidbody2D.linearVelocity = new Vector2(speed * _mGatherInput.Value.x, _mRigidbody2D.linearVelocity.y );
    }

    private void Flip()
    {
        if (_mGatherInput.Value.x * _direction < 0)
        {
            HandleDirection();
        }
    }

    private void HandleDirection()
    {
        mTransform.localScale = new Vector3(-mTransform.localScale.x, mTransform.localScale.y, mTransform.localScale.z);
        _direction *= -1;
    }

    private void JumpForce()
    {
        if (_mGatherInput.IsJumping)
        {
            if (isGrounded)
            {
               _mRigidbody2D.linearVelocity = new Vector2(speed * _mGatherInput.Value.x, jumpForce);
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
        _mGatherInput.IsJumping =  false;
    }

    private void WallJump()
    { 
        _mRigidbody2D.linearVelocity = new Vector2(wallJumpForce.x * -_direction, wallJumpForce.y);
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
        _mRigidbody2D.linearVelocity = new Vector2(speed * _mGatherInput.Value.x, jumpForce); 
        counterExtraJump--;
    }

    public void KnockBack()
    {
        StartCoroutine(KnockBackCoroutine());
        _mRigidbody2D.linearVelocity = new Vector2(knockedPower.x * -_direction,  knockedPower.y);
        _mAnimator.SetTrigger(_idKnockBack);
    }

    IEnumerator KnockBackCoroutine()
    {
        isKnocked = true;
       // canBeKnocked = false;
        yield return new WaitForSeconds(knockedDuration);
        isKnocked = false;
       // canBeKnocked = true;
    }

    public void Die()
    {
        GameObject deathVFX = Instantiate(deathVFXPrefab, mTransform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(mTransform.position, new Vector2(mTransform.position.x + (wallDistanceRay * _direction), mTransform.position.y));
    }
}
