using UnityEngine;

public class CannonController : MonoBehaviour {
    [SerializeField] private LayerMask _mobs;

    private bool _isShooting;
    private Vector3 _playerPos;
    private bool _isDie;

    private CannonballController _cannonball;
    private CannonVisualBoom _visualBoom;

    private void Awake() {
        _cannonball = GetComponentInChildren<CannonballController>();
        _visualBoom = GetComponentInChildren<CannonVisualBoom>();
    }

    private void Update() {
        if (!_isDie) {
            if (!CollisionToDie()) {
                if (!_isShooting) CheckPlayer();
            } else {
                Die();
            }
        }
    }

    public void Reload() {
        _isShooting = false;
    }

    public void DestroyYourself() {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    private void CheckPlayer() {
        if (Player.Instance.CheckGunNear(gameObject)) {
            Shoot();
        }
    }

    private void Shoot() {
        _isShooting = true;
        _visualBoom.On();
        _playerPos = Player.Instance.CurrentPos();
        _cannonball.Shoot(_playerPos);
    }

    private bool CollisionToDie() {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, _mobs);
        if (collider) {
            if (collider.GetComponent<Laser>()) {
                return true;
            }
        }

        return false;
    }

    private void Die() {
        GetComponentInChildren<CannonVisual>().Die();
        _isDie = true;
    }
}
