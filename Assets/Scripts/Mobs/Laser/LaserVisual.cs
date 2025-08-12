using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserVisual : MonoBehaviour {
    private Vector3 _dir;
    int _number;

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

    public void SetData(Vector3 dir, int number) {
        _dir = dir;
        _number = number;
    }

    private bool IsLast() {
        return _laserGun.CheckLaserPosition(_number);
    }
}
