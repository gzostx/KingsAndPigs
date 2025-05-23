using System;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] PlayerController player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            player.Die();
        }
    }
}
