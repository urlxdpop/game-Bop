using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class SpiderController : MonoBehaviour, IMobs {
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _foregroundLayer;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private SpiderVisual _visual;

    private float _waitTime = 0;
    private bool _isMoving;
    private Vector3 _position;
    private bool _isDie;

    private void OnValidate() {
        SetMovingOrientation(_dir);
    }

    private void Update() {
        if (!_isDie) HandleMovement();
    }

    public void DestroyYourself() {
        Destroy(this);
    }

    private void HandleMovement() {
        MoveOrRotate();
    }

    private void SetMovingOrientation(Vector2 dir) {
        transform.rotation = Quaternion.Euler(0, 0, dir.x != 0 ? -dir.x * 90 : dir.y < 0 ? 180 : 0);
    }

    private void MoveOrRotate() {
        if (!_isMoving) {
            if (CheckWall()) {
                Rotate();
            } else {

                _position = transform.position;
                _isMoving = true;
                _waitTime = 0;
                transform.DOMove(transform.position + _dir, _speed).
                    OnUpdate(() => {
                        CheckCollision();
                    }).
                    OnComplete(() => { _isMoving = false; });
            }
        }
    }

    private bool CheckWall() {
        Collider2D collider = Physics2D.OverlapBox(transform.position + _dir, new Vector2(0.5f, 0.5f), _foregroundLayer);

        if (collider) {
            if (collider.gameObject.GetComponent<WebController>() || collider.gameObject.GetComponent<EventButtonController>()) return false;
            if (collider.gameObject.GetComponent<SpikeController>()) Die();
            return true;
        }

        return false;
    }

    private void Rotate() {
        if (_waitTime > _speed) {
            _waitTime = 0;
            _dir = -_dir;
            SetMovingOrientation(_dir);
        } else {
            _waitTime += Time.deltaTime;
        }
    }

    private void CheckCollision() {
        Collider2D collider = Physics2D.OverlapBox(_position + _dir, new Vector2(0.5f, 0.5f), _foregroundLayer);

        if (collider && collider.gameObject != gameObject) {
            if (collider.gameObject.GetComponent<WebController>() || collider.gameObject.GetComponent<EventButtonController>()) return;
            transform.DOKill();
            transform.position = _position;
            _isMoving = false;
        }
    }

    private void Die() {
        _visual.Spider_OnDie();
        transform.DOKill();
        _isDie = true;
    }
}
