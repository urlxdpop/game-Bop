using UnityEngine;
using UnityEngine.Rendering;

public class Laser : MonoBehaviour {
    [SerializeField] private Sprite _expanentedLaser;

    private LaserVisual _laserVisual;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _laserVisual = GetComponentInChildren<LaserVisual>();
    }

    private void Start() {
        RotateLaser();
    }

    private void RotateLaser() {
        if (_laserVisual.IsRotate()) {
            _spriteRenderer.sprite = _expanentedLaser;

            LaserGunController gun = GetComponentInParent<LaserGunController>();
            Vector3 rot = gun.transform.eulerAngles;

            Vector3 dir = _laserVisual.GetDir() + -_laserVisual.GetPastDir();

            if (dir.x == -1) {
                if (dir.y == -1) {
                    transform.localRotation = Quaternion.Euler(0, 0, 0 - rot.z);

                } else if (dir.y == 1) {
                    transform.localRotation = Quaternion.Euler(0, 0, -90 - rot.z);
                } else Debug.LogError("Error Y Rotation");
            } else if (dir.x == 1) {
                if (dir.y == -1) {
                    transform.localRotation = Quaternion.Euler(0, 0, 90 - rot.z);
                } else if (dir.y == 1) {
                    transform.localRotation = Quaternion.Euler(0, 0, 180 - rot.z);
                } else Debug.LogError("Error Y Rotation");
            } else Debug.LogError("Error X Rotation");
        }
    }
}
