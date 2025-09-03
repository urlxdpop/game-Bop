using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

[SelectionBase]
public class BoxController : MonoBehaviour, IEvent, IImpulseObject {
    [SerializeField] private LayerMask _foregroundLayer;

    private Vector3 _dirMoveBox;
    private Vector3 _position;
    private Vector3 _portal;
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

    public bool CanMove(Vector3 player, PortalController portal) {
        if (_isMoving) return false;
        if (portal != null) {
            return false;
        } else {
            _dirMoveBox = transform.position - player;
        }

        if (!CanMovement(transform.position, _dirMoveBox)) return false;
        Teleported(_dirMoveBox);

        return true;
    }

    public bool CanMagnet(Vector3 distance) {
        if (_isMoving) return false;
        _dirMoveBox = distance;

        if (!CanMovement(transform.position, _dirMoveBox)) return false;

        return true;
    }

    public void Impulse(Vector3 dir, float speed) {
        if (_isImpulse || _isMoving) {
            return;
        }

        if (!CanMovement(transform.position, dir)) return;

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

    private bool CanMovement(Vector3 pos, Vector3 dir) {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos + dir, new Vector2(0.5f, 0.5f), 0, _foregroundLayer);
        bool canMove = true;   

        foreach (var collider in colliders) {
            if (!CanWalk(collider)) canMove = false;
            PortalCollision(collider);
        }

        return canMove;
    }

    private void PortalCollision(Collider2D collider) {
        if (collider.GetComponent<PortalController>()) {
            _portal = collider.GetComponent<PortalController>().Teleported();
            
        }
    }

    private void Teleported(Vector3 dir) {
        if (_portal != Vector3.zero) {
            if (CanMovement(_portal, dir)) {
                transform.position = _portal;
                _position = _portal;
            }

            _portal = Vector3.zero;
        }
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
                collider.GetComponent<ImpulseController>() ||
                collider.GetComponent<PortalController>();
    }
}
