using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class SpiderController : MonoBehaviour, IMobs, IImpulseObject {
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _foregroundLayer;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private SpiderVisual _visual;

    private float _waitTime = 0;
    private float _walkTime = 0;
    private bool _isMoving;
    private bool _getImpulse;
    private Vector3 _position;
    private bool _isDie;

    private void OnValidate() {
        SetMovingOrientation(_dir);
    }

    private void Update() {
        if (!_isDie) HandleMovement();
        _getImpulse = false;
    }

    public void DestroyYourself() {
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }

    public void Impulse(Vector3 dir, float speed) {
        bool wall = CheckWall(dir);

        if ((!_isMoving || _walkTime < 2) && !wall) {
            _getImpulse = true;
            Moving(dir);
        } else if (wall) {
            _getImpulse = true;
        }
    }

    private void HandleMovement() {
        MoveOrRotate();
    }

    private void SetMovingOrientation(Vector2 dir) {
        transform.rotation = Quaternion.Euler(0, 0, dir.x != 0 ? -dir.x * 90 : dir.y < 0 ? 180 : 0);
    }

    private void MoveOrRotate() {
        if (!_isMoving && !_getImpulse) {
            if (CheckWall(_dir)) {
                Rotate();
            } else {
                Moving(_dir);
            }
        }
    }

    private void Moving(Vector3 dir) {
        _position = transform.position;
        _isMoving = true;
        _waitTime = 0;
        _walkTime = 0;
        transform.DOMove(transform.position + dir, _speed).
            OnUpdate(() => {
                CheckCollision(dir);
                _walkTime += 1;
                _getImpulse = true;
            }).
            OnComplete(() => { 
                _isMoving = false; 
                _getImpulse = false;
            });
    }

    private bool CheckWall(Vector3 dir) {
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position + dir, new Vector2(0.5f, 0.5f), _foregroundLayer);

        foreach (Collider2D col in collider) {
            if (col) {
                if (WalkToDie(col)) Die();
                if (CanWalk(col)) return false;

                return true;
            }
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

    private void CheckCollision(Vector3 dir) {
        Collider2D[] collider = Physics2D.OverlapBoxAll(_position + dir, new Vector2(0.5f, 0.5f), _foregroundLayer);

        foreach (Collider2D col in collider) {
            if (col && col.gameObject != gameObject && !CanWalk(col)) {
                transform.DOKill();
                transform.position = _position;
                _isMoving = false;
                _getImpulse = false;
            }
        }
        
    }

    private void Die() {
        _visual.Spider_OnDie();
        transform.DOKill();
        _isDie = true;
    }

    private bool CanWalk(Collider2D collider) {
        return collider.GetComponent<EventButtonController>() ||
            collider.GetComponent<SpikeController>() ||
            collider.GetComponent<Laser>() ||
            collider.GetComponent<ImpulseController>();
    }

    private bool WalkToDie(Collider2D collider) {
        if (collider.gameObject.GetComponent<SpikeController>() ||
        collider.gameObject.GetComponent<Laser>()) return true;
        return false;
    }
}
