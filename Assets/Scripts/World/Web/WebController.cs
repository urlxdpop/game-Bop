using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class WebController : MonoBehaviour, IEvent {
    private bool _isDestroy = false;
    
    private Collider2D _collider;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    public void Interact() {
        if (!_isDestroy) {
            _isDestroy = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void BossCreated(Vector3 finalPos) {
        _collider.enabled = false;
        transform.DOMove(finalPos, 0.2f).OnComplete(() => {
            _collider.enabled = true;
            CheckPlayer();
        });
    }

    public bool IsDestroy() {
        return _isDestroy;
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    private void CheckPlayer() {
        if (Player.Instance.CheckCollision(gameObject)) {
            Player.Instance.StunningPlayer();
            BossController.Instance.SharpAttack();
        }
    }
}
