using UnityEngine;

public class DiamondVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Este método será llamado desde el Animation Event
    public void OnHitAnimationEnd()
    {
        spriteRenderer.enabled = false;
    }
}
