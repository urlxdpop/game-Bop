using UnityEngine;

public class ImpulseBlockController : MonoBehaviour {
    [SerializeField] private Vector3 _dir;
    [SerializeField] private LayerMask _foreground;
    [SerializeField] private LayerMask _mobs;
    [SerializeField] private GameObject _impulseObject;

    private Vector3[] _impulse;
    private Vector3[] _impulseDir;
    private ImpulseController[] _Impulse;
    private int _lastImpulse;
    private int _lastImpulseTemp = -1;
    private Vector3 _rotateImpulse;
    private Vector3 _teleportedImpulse;

    private void OnValidate() {
        transform.rotation = SetOrientation(_dir);
    }

    private void Start() {
        _Impulse = new ImpulseController[100];
        TraceImpulsePath();
        SpawnImpulse();
    }

    private void Update() {
        TraceImpulsePath();
        if (_lastImpulseTemp != _lastImpulse) {
            SpawnImpulse();
            _lastImpulseTemp = _lastImpulse == 0 && _lastImpulseTemp == -1 ? -1 : _lastImpulse;
        }
    }

    public Vector3 Dir() {
        return _dir;
    }

    private Quaternion SetOrientation(Vector2 dir) {
        return Quaternion.Euler(0, 0, dir.x != 0 ? -dir.x * 90 : dir.y < 0 ? 180 : 0);
    }

    private void TraceImpulsePath() {
        Vector3[] impulse = new Vector3[100];
        Vector3[] impulseDir = new Vector3[100];

        Vector3 dir = _dir.normalized;
        impulseDir[0] = dir;
        Vector3 position = transform.position;

        _lastImpulse = 0;
        int i = 0;
        while (i < 99) {
            Vector3 nextPos = position + dir;

            if (HaveColliderConnect(nextPos, dir)) {
                if (_rotateImpulse != Vector3.zero) {
                    dir = _rotateImpulse.normalized;
                    _rotateImpulse = Vector3.zero;
                } else if (_teleportedImpulse == Vector3.zero) {
                    break;
                }
            }

            impulse[i] = nextPos;
            impulseDir[i] = dir;
            _lastImpulse = i;
            i++;

            if (_teleportedImpulse != Vector3.zero) {
                position = _teleportedImpulse;
                _teleportedImpulse = Vector3.zero;
            } else {
                position = nextPos;
            }
        }

        if (i == 0) {
            _lastImpulseTemp = -1;
        }

        _impulse = impulse;
        _impulseDir = impulseDir;
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
            _teleportedImpulse = col.GetComponent<PortalController>().Teleported();
            return true;
        }

        return false;
    }

    private bool IgnoreObject(Collider2D collider) {
        return collider.GetComponent<IImpulseObject>() != null ||
            collider.GetComponent<ImpulseController>();
    }

    private void SpawnImpulse() {
        RemoveImpulse();

        if (_impulse[0] == Vector3.zero) {
            return;
        }

        for (int i = 0; i < _lastImpulse + 1; i++) {
            GameObject laserObj = Instantiate(_impulseObject, _impulse[i], Quaternion.identity, transform);

            ImpulseController impulseComponent = laserObj.GetComponent<ImpulseController>();
            _Impulse[i] = impulseComponent;
            _Impulse[i].transform.rotation = SetOrientation(_impulseDir[i]);
            _Impulse[i].SetDir(_impulseDir[i].normalized);
        }

        if (_lastImpulseTemp == -1) {
            _lastImpulseTemp = _lastImpulse;
        }
    }

    private void RemoveImpulse() {
        for (int i = 0; i < _Impulse.Length; i++) {
            if (_Impulse[i] != null) {
                Destroy(_Impulse[i].gameObject);
                _Impulse[i] = null;
            }
        }
    }

    private bool CheckMirror(Collider2D collider, Vector3 impulseDir) {
        var mirror = collider.GetComponent<MirrorController>();
        if (mirror != null) {
            Vector3 dir = mirror.RotateRay(impulseDir);
            if (dir != Vector3.zero) {
                _rotateImpulse = dir.normalized;
                return true;
            }
        }
        return false;
    }
}
