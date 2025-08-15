using DG.Tweening;
using System;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }

    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private LayerMask _foregroundLayer;
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private LayerMask _mobsLayer;
    [SerializeField] private Skills _skills;
    [SerializeField] private int _hp;

    public event EventHandler OnRotate;

    private bool _isMoving;
    private Vector2 _inputVector;
    private string _lastButton;
    private GameObject _currentInteractableObject;
    private Vector2 _currentPos;
    private float _timeInvulneradility;
    private float _timeEndInvulneradility;
    private bool _invulneradility;
    private int _maxHp;

    private void Awake() {
        Instance = this;
        _maxHp = _hp;
    }

    private void Start() {
        _lastButton = "x";
        _currentPos = transform.position;
        _timeEndInvulneradility = 1f;
    }

    public void HandlerUpdate() {
        if (!_isMoving) {
            PlayerMovement();
        }

        CheckMobs();
        SkillsActivated();
    }

    public Vector2 InputVector() {
        return _inputVector;
    }

    public bool IsMoving() {
        return _isMoving;
    }

    public Vector2 CurrentPos() {
        return _currentPos;
    }

    public GameObject CurrentInteractableObject() {
        return _currentInteractableObject;
    }

    public float Speed() {
        return _speed;
    }

    public bool CheckCollision(GameObject gameObject) {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, _foregroundLayer);
        if (collider) return collider.gameObject == gameObject;
        return false;
    }

    public int GetHP() {
        return _hp;
    }

    public int GetMaxHP() {
        return _maxHp;
    }

    private void PlayerMovement() {
        _inputVector = GameInput.Instance.GetMovementAction();
        SetDominantOrientation();
        if (_inputVector != Vector2.zero) {
            OnRotate?.Invoke(this, EventArgs.Empty);
            Vector3 targetPos = transform.position;

            targetPos.x += _inputVector.x;
            targetPos.y += _inputVector.y;

            if (IsWalkable(targetPos)) Move(targetPos);
        }

    }

    private void SetDominantOrientation() {
        if (_inputVector.x != 0 && _inputVector.y != 0) {
            if (_lastButton == "x") {
                _inputVector.x = 0;
                _inputVector.y = Mathf.Ceil(_inputVector.y);
                _lastButton = "y";
            } else {
                _inputVector.y = 0;
                _inputVector.x = Mathf.Ceil(_inputVector.x);
                _lastButton = "x";
            }
        } else if (_inputVector.x != 0) {
            _lastButton = "x";
        } else {
            _lastButton = "y";
        }
    }

    private void SkillsActivated() {
        if (Input.GetKey(KeyCode.Space)) {
            if (!_skills.IsActive()) {
                _skills.Destroy();
                CheckForDestroy();
            }
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            if (!_skills.IsActive()) {
                _skills.Magnet();
                CheckForMagnet();
            }
        }
    }

    private bool IsWalkable(Vector3 targetPos) {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.2f, _foregroundLayer);
        if (collider) {
            if (CanWalk(collider)) return true;
            if (WebCollider(collider)) return true;
            if (MoveBox(collider)) return true;
            SpikeCollider(collider);
            return false;
        }
        return true;
    }

    private void Move(Vector3 targetPos) {
        _isMoving = true;
        _currentPos = targetPos;

        transform.DOMove(targetPos, _speed)
            .OnComplete(() => {
                transform.position = targetPos;
                _isMoving = false;

                CheckForEncounters();
            });
    }
    
    private bool CanWalk(Collider2D collider) {
        return collider.GetComponent<ImpulseController>();
    }

    private void CheckForEncounters() {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.01f, _interactLayer);
        if (collider) {
            GameObject gameObject = collider.gameObject;
            _currentInteractableObject = gameObject;

            gameObject.GetComponent<IEvent>()?.Interact();
        } else {
            _currentInteractableObject = null;
        }
    }

    private void CheckForDestroy() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_currentPos, new Vector2(2, 2), _foregroundLayer);

        foreach (Collider2D collider in colliders) {
            GameObject gameObject = collider.gameObject;
            if (gameObject.GetComponent<CollipsibleBlockController>()) gameObject.GetComponent<CollipsibleBlockController>().GetComponent<IEvent>().Interact();
        }
    }

    private void CheckForMagnet() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_currentPos, new Vector2(4, 4), _foregroundLayer);

        foreach (Collider2D collider in colliders) {
            GameObject gameObject = collider.gameObject;
            if (gameObject.GetComponent<BoxController>()) {
                Vector3 distance = IsTrueDistance((Vector3)_currentPos - gameObject.transform.position);
                if (distance != Vector3.zero) {
                    if (gameObject.GetComponent<BoxController>().CanMagnet(distance)) {
                        gameObject.GetComponent<BoxController>().GetComponent<IEvent>().Interact();
                    }
                }
            }
        }
    }

    private void CheckMobs() {
        if (_invulneradility) {
            _timeInvulneradility += Time.deltaTime;
            if (_timeInvulneradility >= _timeEndInvulneradility) {
                _timeInvulneradility = 0;
                _invulneradility = false;
            }
        } else {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, _mobsLayer);

            if (collider) {
                TakeDamage();
            }
        }
    }

    private Vector3 IsTrueDistance(Vector3 distance) {
        if (Mathf.Abs(distance.x) == 2 || Mathf.Abs(distance.y) == 2) {
            if (Mathf.Abs(distance.x) == 2 && Mathf.Abs(distance.y) == 2) {
                return Vector3.zero;
            } else {
                if (Mathf.Abs(distance.x) == 2) {
                    return new Vector3(distance.x / 2, 0);
                } else if (Mathf.Abs(distance.y) == 2) {
                    return new Vector3(0, distance.y / 2);
                }
            }
        }
        return Vector3.zero;
    }

    private bool WebCollider(Collider2D collider) {
        if (collider.gameObject.GetComponent<WebController>()) {
            collider.gameObject.GetComponent<WebController>().GetComponent<IEvent>().Interact();
            return true;
        }
        return false;
    }

    private bool MoveBox(Collider2D collider) {
        BoxController box = collider.gameObject.GetComponent<BoxController>();
        if (box) {
            if (box.CanMove(transform.position)) {
                box.GetComponent<IEvent>()?.Interact();
                return true;
            }
        }
        return false;
    }

    private void SpikeCollider(Collider2D collider) {
        SpikeController spike = collider.gameObject.GetComponent<SpikeController>();
        if (spike) {
            SpikeDamage(spike);
        }
    }

    private void TakeDamage() {
        if (!_invulneradility) {
            _hp--;
            _invulneradility = true;
        }
    }

    private void SpikeDamage(SpikeController spike) {
        Vector2 dir = spike.Orientation();
        if (-dir == _inputVector) {
            TakeDamage();
        } else {
            TakeDamage();

            if (IsWalkable(transform.position + (Vector3)_inputVector + (Vector3)dir)) {
                transform.position += (Vector3)_inputVector;
                Move(transform.position + (Vector3)dir);
            }
        }
    }
}
