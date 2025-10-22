using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserVisual : MonoBehaviour {
    private Vector3 _dir;
    private Vector3 _pastDir;
    private bool _isRatate;
    private int _number;

    private Animator _animator;
    private LaserGunController _laserGun;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _laserGun = GetComponentInParent<LaserGunController>();
    }

    private void Start() {
        _animator.enabled = false;
    }

    private void Update() {
        _animator.enabled = IsLast();
    }

    public bool IsRotate() {
        return _isRatate;
    }

    public Vector3 GetDir() {
        return _dir;
    }

    public Vector3 GetPastDir() {
        return _pastDir;
    }

    public void SetData(Vector3 dir, int number, Vector3 pastDir) {
        _dir = dir.normalized;
        _pastDir = pastDir.normalized;
        _number = number;

        if (_pastDir != _dir) {
            _isRatate = true;
        } else {
            _isRatate = false;
        }

        if (!_isRatate) {
            return;
        }
        int angle = dir.x != 0 ? dir.x == pastDir.y ? -90 : 180 : dir.y == pastDir.x ? 180 : -90;

        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private bool IsLast() {
        return _laserGun.CheckLaserPosition(_number);
    }
}
