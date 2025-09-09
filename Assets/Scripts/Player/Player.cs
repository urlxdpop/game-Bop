using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }

    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private LayerMask _foregroundLayer;
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private LayerMask _mobsLayer;
    [SerializeField] private Skills _skills;
    [SerializeField] private GameObject _oxygenVisual;
    [SerializeField] private int _hp;

    public event EventHandler OnRotate;

    private bool _isMoving;
    private Vector2 _inputVector;
    private string _lastButton;
    private GameObject _currentInteractableObject;
    private Vector2 _currentPos;
    private Vector3 _position;

    private float _timeInvulneradility;
    private float _timeEndInvulneradility;
    private bool _invulneradility;
    private int _maxHp;

    private int _oxygen;
    private bool _inWater;
    private float _timeOxygen = 2f;
    private float _timeCurrentOxygen;

    private bool _inPortal;
    private PortalController _portal;

    private void Awake() {
        Instance = this;
        _maxHp = _hp;
        _oxygen = 5;
        _timeCurrentOxygen = _timeOxygen;
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
        CheckWater();
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

    public int GetOxygen() {
        return _oxygen;
    }

    public int GetMaxHP() {
        return _maxHp;
    }

    public void InWater() {
        _inWater = true;
    }

    public void Die() {
        DOTween.KillAll();
        DOTween.Clear(true);
        GameInput.Instance.Disable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayerMovement() {
        _portal = null;
        _inputVector = GameInput.Instance.GetMovementAction();

        SetDominantOrientation();

        if (_inputVector != Vector2.zero) {
            OnRotate?.Invoke(this, EventArgs.Empty);
            Vector3 targetPos = transform.position;

            targetPos.x += _inputVector.x;
            targetPos.y += _inputVector.y;

            if (IsWalkable(targetPos, false, false)) {
                if (_portal != null) {
                    Teleported();
                }
                Move(targetPos);
            }
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
                _skills.DestroyYourself();
                CheckForDestroy();
            }
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            if (!_skills.IsActive()) {
                _skills.Magnet();
                CheckForMagnet();
            }
        }
    }

    private bool IsWalkable(Vector3 targetPos, bool inSpike, bool inPortal) {
        Collider2D[] collider = Physics2D.OverlapCircleAll(targetPos, 0.2f, _foregroundLayer);

        bool canWalk = true;

        foreach (Collider2D col in collider) {
            if (col) {
                if (!inPortal) PortalCollider(col);
                if (WebCollider(col)) canWalk = false;
                if (MoveBox(col)) canWalk = false;
                if (SpikeCollider(col, inSpike)) canWalk = false;
                if (!CanWalk(col)) canWalk = false;
            }
        }


        return canWalk;
    }

    private void Move(Vector3 targetPos) {
        if (_inPortal) {
            _inPortal = false;
            return;
        }

        _isMoving = true;
        _currentPos = targetPos;
        _position = transform.position;

        transform.DOMove(targetPos, _speed)
            .OnUpdate(() => {
                if (StopMove()) {
                    transform.position = _position;
                    _isMoving = false;
                    transform.DOKill();
                    return;
                }
            })
            .OnComplete(() => {
                transform.position = targetPos;
                _isMoving = false;

                CheckForEncounters();
            });
    }

    private bool StopMove() {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.01f, _foregroundLayer);

        if (collider) {
            if (CanWalk(collider)) return false;
            return true;
        }

        return false;
    }

    private bool CanWalk(Collider2D collider) {
        return collider.GetComponent<ImpulseController>() ||
            collider.GetComponent<WebController>() ||
            collider.GetComponent<SpikeController>() ||
            collider.GetComponent<PortalController>();
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

    private void PortalCollider(Collider2D collider) {
        if (collider.gameObject.GetComponent<PortalController>()) {
            _portal = collider.gameObject.GetComponent<PortalController>();
        }
    }

    private bool MoveBox(Collider2D collider) {
        BoxController box = collider.gameObject.GetComponent<BoxController>();
        if (box) {
            if (box.CanMove(transform.position, _portal)) {
                box.GetComponent<IEvent>()?.Interact();
                return true;
            }
        }
        return false;
    }

    private bool SpikeCollider(Collider2D collider, bool inSpike) {
        SpikeController spike = collider.gameObject.GetComponent<SpikeController>();
        if (spike) {
            SpikeDamage(spike, inSpike);
            return true;
        }
        return false;
    }

    private void TakeDamage() {
        if (!_invulneradility) {
            _hp--;
            _invulneradility = true;
        }
        if (_hp <= 0) {
            Die();
        }
    }

    private void SpikeDamage(SpikeController spike, bool inSpike) {
        Vector2 dir = spike.Orientation();

        if (-dir == _inputVector) {
            TakeDamage();
        } else {
            TakeDamage();

            if (!inSpike && IsWalkable(transform.position + (Vector3)_inputVector + (Vector3)dir, true, false)) {
                transform.position += (Vector3)_inputVector;
                Move(transform.position + (Vector3)dir);
            }
        }
    }

    private void Teleported() {
        Vector3 pos = _portal.Teleported();

        if (IsWalkable(pos, false, true)) {
            _inPortal = true;
            transform.position = pos;
            _currentPos = pos;
        }
    }

    private void CheckWater() {

        if (_inWater) {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0.1f, _interactLayer);

            bool isInWater = false;
            foreach (Collider2D collider in colliders) {
                if (collider.gameObject.GetComponent<WaterController>()) {
                    isInWater = true;
                }
            }

            if (!isInWater) {
                _inWater = false;
            } else {
                if (_oxygen > 0) {
                    _timeCurrentOxygen -= Time.deltaTime;
                    if (_timeCurrentOxygen <= 0) {
                        _oxygen--;
                        _timeCurrentOxygen = _timeOxygen;
                    }
                } else {
                    TakeDamage();
                    _timeCurrentOxygen = _timeOxygen;
                }
            }
        } else {
            _oxygen = 5;
        }

        _oxygenVisual.SetActive(_inWater);
    }
}

