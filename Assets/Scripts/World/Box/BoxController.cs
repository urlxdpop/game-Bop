using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class BoxController : MonoBehaviour, IEvent {
    [SerializeField] private LayerMask _foregroundLayer;

    private Vector3 _dirMoveBox;
    private Vector3 _position;
    private bool _isMoving;

    public void Interact() {
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

    public bool CanMove(Vector3 player) {
        if (_isMoving) return false;
        _dirMoveBox = transform.position - player;

        Collider2D collider = Physics2D.OverlapBox(transform.position + _dirMoveBox, new Vector2(0.5f, 0.5f), _foregroundLayer);
        if (collider) {
            if (CanWalk(collider)) return true;
            return false;
        }

        return true;
    }

    public bool CanMagnet(Vector3 distance) {
        if (_isMoving) return false;
        _dirMoveBox = distance;

        Collider2D collider = Physics2D.OverlapBox(transform.position + distance, new Vector2(0.5f, 0.5f), _foregroundLayer);
        if (collider) {
            if (CanWalk(collider)) return true;
            return false;
        }

        return true;
    }

    private void CheckCollision() {
        Collider2D collider = Physics2D.OverlapBox(_position + _dirMoveBox, new Vector2(0.5f, 0.5f), _foregroundLayer);
        
        if (collider && collider.gameObject != gameObject || Player.Instance.CheckCollision(gameObject)) {
            if (CanWalk(collider)) return;
            transform.DOKill();
            transform.position = _position;
            _isMoving = false;
        }
    }

    private bool CanWalk(Collider2D collider) {
        if (collider.gameObject.GetComponent<EventButtonController>() ||
            collider.gameObject.GetComponent<Laser>()) return true;
        return false;
    }
}
