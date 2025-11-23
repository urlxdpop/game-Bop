using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class SpiderController : MonoBehaviour, IMobs, IImpulseObject {
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _foregroundLayer;
    [SerializeField] private Direction _direction;
    [SerializeField] private SpiderVisual _visual;

    private float _waitTime = 0;
    private float _walkTime = 0;
    private bool _isMoving;
    private bool _getImpulse;
    private Vector3 _position;
    private Vector3 _teleportedPos;
    private Vector3 _dir;
    private bool _isDie;
    private bool _rotate;

    private Collider2D _collider;

    private void OnValidate() {
        _dir = SetDir(_direction);
        SetMovingOrientation(_dir, false);
    }

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    private void Start() {
        _dir = SetDir(_direction);
        SetMovingOrientation(_dir, false);
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
        bool wall = CheckWall(dir, false);
        _getImpulse = true;
        
        if ((!_isMoving || _walkTime < 2) && !wall) {
            Moving(dir);
        }
    }

    public void BossCreated(Vector3 finalPos) {
        _collider.enabled = false;
        transform.DOMove(finalPos, 0.2f).OnComplete(() => {
            _collider.enabled = true;
        });
    }

    public void SetMovingOrientation(Vector3 dir, bool setDirection) {
        _dir = dir;
        if (setDirection) {
            _direction = SetDirection(dir);
        }

        transform.rotation = Quaternion.Euler(0, 0, dir.x != 0 ? -dir.x * 90 : dir.y < 0 ? 180 : 0);
    }

    public void Die() {
        _visual.Spider_OnDie();
        transform.DOKill();
        _isDie = true;
    }

    private Vector3 SetDir(Direction dir) {
        return dir switch {
            Direction.UP => Vector3.up,
            Direction.DOWN => Vector3.down,
            Direction.LEFT => Vector3.left,
            Direction.RIGHT => Vector3.right,
            _ => Vector3.zero,
        };
    }

    private Direction SetDirection(Vector3 dir) {
        if (dir == Vector3.up) return Direction.UP;
        if (dir == Vector3.down) return Direction.DOWN;
        if (dir == Vector3.left) return Direction.LEFT;
        if (dir == Vector3.right) return Direction.RIGHT;
        return default;
    }


    private void HandleMovement() {
        if (_collider.enabled) {
            MoveOrRotate();
            CheckDie();
        }
    }

    private void MoveOrRotate() {
        if (!_isMoving && !_getImpulse) {
            if (CheckWall(_dir, false) || _rotate) {
                Rotate();
            } else {
               
                if (_teleportedPos != Vector3.zero) Teleported();
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
            }).
            OnComplete(() => {
                _isMoving = false;
                _getImpulse = false;
            });
    }

    private bool CheckWall(Vector3 dir, bool inPortal) {
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position + dir, new Vector2(0.5f, 0.5f), _foregroundLayer);
        bool wall = false;

        foreach (Collider2D col in collider) {
            if (col) {
                if (!CanWalk(col)) wall = true;
                if (!inPortal) PortalCollision(col);
            }
        }

        return wall;
    }

    private void Rotate() {
        if (_waitTime > _speed + Random.Range(0, 0.1f)) {
            _waitTime = 0;
            _dir = -_dir;
            _rotate = false;
            SetMovingOrientation(_dir, false);
        } else {
            _waitTime += Time.deltaTime;
        }
    }

    private void PortalCollision(Collider2D collider) {
        if (collider.GetComponent<PortalController>()) {
            _teleportedPos = collider.GetComponent<PortalController>().Teleported();
            Teleported();
        }
    }

    private void Teleported() {
        transform.position = _teleportedPos;
        _position = _teleportedPos;
        _teleportedPos = Vector3.zero;
    }

    private void CheckCollision(Vector3 dir) {
        Collider2D[] collider = Physics2D.OverlapBoxAll(_position + dir, new Vector2(0.5f, 0.5f), 0f);

        foreach (Collider2D col in collider) {
            if (col && col.gameObject != gameObject && !CanWalk(col)) {
                transform.DOKill();
                transform.position = _position;
                _isMoving = false;
                _getImpulse = false;
                _rotate = true;
            }
        }

    }

    private bool CanWalk(Collider2D collider) {
        return collider.GetComponent<EventButtonController>() ||
            collider.GetComponent<SpikeController>() ||
            collider.GetComponent<Laser>() ||
            collider.GetComponent<ImpulseController>() ||
            collider.GetComponent<PortalController>() ||
            collider.GetComponent<WebController>() ||
            collider.GetComponent<SpiderBossController>();
    }

    private bool WalkToDie(Collider2D collider) {
        if (collider.gameObject.GetComponent<SpikeController>() ||
        (collider.gameObject.GetComponent<Laser>() && !GetComponent<RedSpiderController>())) return true;
        else if (collider.GetComponent<SpiderBossController>()) {
            if (GetComponent<RedSpiderController>()) {
                collider.GetComponent<SpiderBossController>().TakeDamage();
            }
            return true;
        }
        return false;
    }

    private void CheckDie() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0f);

        foreach (Collider2D col in colliders) {
            if (col && col.gameObject != gameObject) {
                if (WalkToDie(col)) Die();
            }
        }
    }
}
