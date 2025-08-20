
using UnityEngine;

[SelectionBase]
public class LaserGunController : MonoBehaviour {
    [SerializeField] private Vector3 _dir;
    [SerializeField] private LayerMask _foreground;
    [SerializeField] private LayerMask _mobs;
    [SerializeField] private GameObject _laser;

    private Vector3[] _lasers;
    private Vector3[] _lasersDir;
    private Laser[] _Lasers;
    private int _lastLaser;
    private int _lastLaserTemp = -1;
    private bool _isDie;

    private void OnValidate() {
        SetOrientation(_dir);
    }

    private void Start() {
        _Lasers = new Laser[100];
        TraceLaserPath();
        SpawnLasers();
    }

    private void Update() {
        if (!_isDie) {
            if (!CollisionToDie()) {
                TraceLaserPath();
                if (_lastLaserTemp != _lastLaser) {
                    SpawnLasers();
                    _lastLaserTemp = _lastLaser == 0 && _lastLaserTemp == -1 ? -1 : _lastLaser;
                }
            }
        }
    }

    public bool CheckLaserPosition(int number) {
        return number == _lastLaser;
    }

    public bool IsDie() {
        return _isDie;
    }

    public void DestroyYourself() {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    private void SetOrientation(Vector2 dir) {
        transform.rotation = Quaternion.Euler(0, 0, dir.y != 0 ? dir.y * 90 : dir.x < 0 ? 180 : 0);
    }

    private void TraceLaserPath() {
        Vector3[] lasers = new Vector3[100];
        Vector3[] lasersDir = new Vector3[100];

        lasersDir[0] = _dir;
        Vector3 position = transform.position;

        _lastLaser = 0;
        int i = 0;
        while (i < 99) {
            Vector3 nextPos = position + lasersDir[i];

            if (HaveColliderConnect(nextPos)) break;

            lasers[i] = nextPos;
            lasersDir[i + 1] = _dir;
            position = nextPos;
            _lastLaser = i;
            i++;
        }

        if (i == 0) {
            _lastLaserTemp = -1;
        }

        _lasers = lasers;
        _lasersDir = lasersDir;
    }

    private bool HaveColliderConnect(Vector3 nextPos) {
        Collider2D[] collider = Physics2D.OverlapCircleAll(nextPos, 0.2f, _foreground);

        foreach (var col in collider) {
            if (col && !IgnoreObject(col)) {
                return true;
            }
        }

        return false;
    }

    private void SpawnLasers() {
        RemoveLasers();

        if (_lasers[0] == Vector3.zero) {
            return;
        }

        for (int i = 0; i < _lastLaser + 1; i++) {
            GameObject laserObj = Instantiate(_laser, _lasers[i], Quaternion.LookRotation(_lasersDir[i]), transform);
            laserObj.transform.right = _lasersDir[i];

            _Lasers[i] = laserObj.GetComponent<Laser>();
            _Lasers[i].GetComponentInChildren<LaserVisual>().SetData(_lasersDir[i], i);
        }

        if (_lastLaserTemp == -1) {
            _lastLaserTemp = _lastLaser;
        }
    }

    private void RemoveLasers() {
        for (int i = 0; i < _Lasers.Length; i++) {
            if (_Lasers[i] != null) {
                Destroy(_Lasers[i].gameObject);
                _Lasers[i] = null;
            }
        }
    }

    private bool CollisionToDie() {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.2f, _mobs);
        if (collider) {
            if (collider.GetComponent<Laser>()) {
                Die();
                return true;
            }
        }

        return false;
    }

    private bool IgnoreObject(Collider2D collider) {
        return collider.GetComponent<ImpulseController>();
    }

    private void Die() {
        RemoveLasers();
        GetComponentInChildren<LaserGunVisual>().Laser_OnDie();
        _isDie = true;
    }
}