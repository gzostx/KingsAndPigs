using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Diamond : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Rigidbody2D mRigidbody2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private DiamondType diamondType;
    private int _idHitDiamond;
    private int  _idDiamondIndex;

    private void Awake()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        _idDiamondIndex = Animator.StringToHash("DiamondIndex");
        _idHitDiamond = Animator.StringToHash("Hit");
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        SetRandomDiamond();

    }

    private void SetRandomDiamond()
    {
        if (!gameManager.DiamondHaveRandomLook())
        {
            UpdateDiamondType();
            return;
        }
        var randomDiamondIndex = Random.Range(0, 6);
        animator.SetFloat(_idDiamondIndex, randomDiamondIndex);
    }

    private void UpdateDiamondType()
    {
        animator.SetFloat(_idDiamondIndex,(int)diamondType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mRigidbody2D.simulated = false;
            gameManager.AddDiamond();
            animator.SetTrigger(_idHitDiamond);
        }
    }
}
