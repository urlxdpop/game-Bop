using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class CannonballController : MonoBehaviour {
    private Animator _animator;
    private Collider2D _collider;

    private Vector3 _position;
    private CannonController _cannon;

    private const string BOOM = "Boom";

    private void Awake() {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _cannon = GetComponentInParent<CannonController>();
    }

    private void Start() {
        _collider.enabled = false;
        gameObject.SetActive(false);

        _position = transform.position;
    }

    public void Shoot(Vector3 pos) {
        gameObject.SetActive(true);
        transform.position = _position;

        transform.DOMove(pos, 1f);
        transform.DOScale(2f, 0.5f).OnComplete(() => {
            transform.DOScale(1f, 0.5f).OnComplete(() => {
                _collider.enabled = true;
                _animator.SetTrigger(BOOM);
            });
        });
    }

    public void Off() {
        _collider.enabled = false;
        _cannon.Reload();
        gameObject.SetActive(false);
    }
}
