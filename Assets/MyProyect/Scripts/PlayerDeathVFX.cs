using UnityEngine;

public class PlayerDeathVFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDeathAnimationEnd()
    {
        _spriteRenderer.enabled = false;
        Destroy(gameObject);  // si deseas eliminar el jugador de la escena
    }
}

