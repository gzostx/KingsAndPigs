using UnityEngine;

public class IntensiveDamage : MonoBehaviour
{
    [SerializeField] private Vector2 extraKnockedPower;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        player.KnockedPower = extraKnockedPower;
        player.KnockBack();
        
    }
}
