using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class BoxController : MonoBehaviour, IEvent, IImpulseObject {
    [SerializeField] private LayerMask _foregroundLayer;

    private Vector3 _dirMoveBox;
    private Vector3 _position;
    private bool _isMoving;
    private bool _isImpulse;

    public void Interact() {
        if (_isImpulse) {
            transform.position = _position;
            _isImpulse = false;
        }
        _position = transform.position;
        _isMoving = true;
        transform.DOMove(transform.position + _dirMoveBox, Player.Instance.Speed())
             .OnUpdate(() => {
                 CheckCollision();
             })
             .OnComplete(() => {
                 _isMoving = false;
             });
    }

    public bool IsMoving() {
        return _isMoving;
    }

    public bool CanMove(Vector3 player) {
        if (_isMoving) return false;
        _dirMoveBox = transform.position - player;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + _dirMoveBox, new Vector2(0.5f, 0.5f), 0, _foregroundLayer);
        foreach (var collider in colliders) {
            if (CanWalk(collider)) return true;
            return false;
        }

        return true;
    }

    public bool CanMagnet(Vector3 distance) {
        if (_isMoving) return false;
        _dirMoveBox = distance;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + distance, new Vector2(0.5f, 0.5f), 0, _foregroundLayer);
        foreach (var collider in colliders) {
            if (CanWalk(collider)) return true;
            return false;
        }

        return true;
    }

    public void Impulse(Vector3 dir, float speed) {
        if (_isImpulse || _isMoving) {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + dir, new Vector2(0.5f, 0.5f), 0, _foregroundLayer);
        foreach (var collider in colliders) {
            if (!CanWalk(collider)) {
                return;
            }
        }

        _isImpulse = true;
        _dirMoveBox = dir;
        _position = transform.position;

        transform.DOMove(transform.position + _dirMoveBox, speed)
             .OnUpdate(() => {
                 CheckCollision();
             })
             .OnComplete(() => {
                 _isMoving = false;
                 _isImpulse = false;
             });
    }

    private void CheckCollision() {
        Collider2D[] collider = Physics2D.OverlapBoxAll(_position + _dirMoveBox, new Vector2(0.5f, 0.5f), _foregroundLayer);

        foreach (Collider2D col in collider) {
            if (col.gameObject != gameObject && !CanWalk(col) || Player.Instance.CheckCollision(gameObject)) {
                transform.DOKill();
                transform.position = _position;
                _isMoving = false;
                _isImpulse = false;
                return;
            }
        }

    }

    private bool CanWalk(Collider2D collider) {
        return collider.GetComponent<EventButtonController>() ||
                collider.GetComponent<Laser>() ||
                collider.GetComponent<ImpulseController>();
    }
}
