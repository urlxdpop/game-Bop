using System;
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
    private Vector3 _rotateLaser;
    private Vector3 _teleportedLaser;

    private void OnValidate() {
        transform.rotation = SetOrientation(_dir);
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

    public Vector3 Dir() {
        return _dir;
    }

    public bool IsDie() {
        return _isDie;
    }

    public void DestroyYourself() {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    private Quaternion SetOrientation(Vector2 dir) {
        return Quaternion.Euler(0, 0, dir.y != 0 ? dir.y * 90 : dir.x < 0 ? 180 : 0);
    }

    private void TraceLaserPath() {
        Vector3[] lasers = new Vector3[100];
        Vector3[] lasersDir = new Vector3[100];

        Vector3 dir = _dir.normalized;
        lasersDir[0] = dir;
        Vector3 position = transform.position;

        _lastLaser = 0;
        int i = 0;
        while (i < 99) {
            Vector3 nextPos = position + dir;

            if (HaveColliderConnect(nextPos, dir)) {
                if (_rotateLaser != Vector3.zero) {
                    dir = _rotateLaser.normalized;
                    _rotateLaser = Vector3.zero;
                } else if (_teleportedLaser != Vector3.zero) {
                } else {
                    break;
                }
            }

            lasers[i] = nextPos;
            lasersDir[i] = dir;
            _lastLaser = i;
            i++;

            if (_teleportedLaser != Vector3.zero) {
                position = _teleportedLaser;
                _teleportedLaser = Vector3.zero;
            } else {
                position = nextPos;
            }
        }

        if (i == 0) {
            _lastLaserTemp = -1;
        }

        _lasers = lasers;
        _lasersDir = lasersDir;
    }

    private bool HaveColliderConnect(Vector3 nextPos, Vector3 laserDir) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(nextPos, 0.2f, _foreground);

        foreach (var col in colliders) {
            if (CheckMirror(col, laserDir)) return true;
            if (CheckPortal(col)) return true;
            if (IgnoreObject(col)) continue;
            return true;
        }

        return false;
    }

    private bool CheckPortal(Collider2D col) {
        if (col.GetComponent<PortalController>()) {
            _teleportedLaser = col.GetComponent<PortalController>().Teleported();
            return true;
        }

        return false;
    }

    private void SpawnLasers() {
        RemoveLasers();

        if (_lasers[0] == Vector3.zero) {
            return;
        }

        for (int i = 0; i < _lastLaser + 1; i++) {
            GameObject laserObj = Instantiate(_laser, _lasers[i], Quaternion.identity, transform);

            Laser laserComponent = laserObj.GetComponent<Laser>();
            _Lasers[i] = laserComponent;
            _Lasers[i].transform.rotation = SetOrientation((Vector2)_lasersDir[i]);
            LaserVisual visual = laserObj.GetComponentInChildren<LaserVisual>();
            visual.SetData(_lasersDir[i].normalized, i, i == 0 ? _dir : _lasersDir[i - 1]);
        }

        if (_lastLaserTemp == -1) {
            _lastLaserTemp = _lastLaser;
        }
    }

    private bool CheckMirror(Collider2D collider, Vector3 laserDir) {
        if (collider.GetComponent<MirrorController>()) {
            Vector3 dir = collider.GetComponent<MirrorController>().RotateRay(laserDir);
            if (dir != Vector3.zero) {
                _rotateLaser = dir;
                return true;
            }
        }

        return false;
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