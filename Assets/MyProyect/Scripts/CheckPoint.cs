using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("Active");
    [SerializeField] private Animator animator;
    [SerializeField] private bool isActive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive) return;
        if (other.CompareTag("Player"))
        {
            ActiveCheckPoint();
        }
        GameManager.Instance.hasCheckPointActive = true;
        GameManager.Instance.checkPoinRespawnPosition = this.transform.position;
        
    }

    private void ActiveCheckPoint()
    {
        isActive = true;
        animator.SetTrigger(Active);
    }
}
