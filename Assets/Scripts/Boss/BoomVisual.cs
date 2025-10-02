using UnityEngine;

public class BoomVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Off() {
        _spriteRenderer.enabled = false;
        Destroy(gameObject);
    }
}
