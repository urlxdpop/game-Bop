using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        Instantiate(_player, transform.position, transform.rotation);
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (_spriteRenderer.enabled) _spriteRenderer.enabled = false;
    }
}
