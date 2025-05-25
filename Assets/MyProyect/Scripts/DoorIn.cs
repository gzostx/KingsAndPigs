using System;
using UnityEngine;

public class DoorIn : MonoBehaviour
{
    private static readonly int IdOpenDoor = Animator.StringToHash("Open");
    private Animator animator => GetComponent<Animator>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Instance.hasCheckPointActive) return;
        if (!other.CompareTag("Player")) return;
        animator.SetTrigger(IdOpenDoor);
        other.GetComponent<PlayerController>().DoorIn();
        other.transform.position = new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z);
        
    }
}
