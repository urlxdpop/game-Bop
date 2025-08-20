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

    private void OnValidate() {
        SetOrientation(_dir);
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

    private void SetOrientation(Vector2 dir) {
        transform.rotation = Quaternion.Euler(0, 0, dir.x != 0 ? -dir.x * 90 : dir.y < 0 ? 180 : 0);
    }

    private void TraceImpulsePath() {
        Vector3[] impulseBlocks = new Vector3[100];
        Vector3[] impulseBlocksDir = new Vector3[100];

        impulseBlocksDir[0] = _dir;
        Vector3 position = transform.position;

        _lastImpulse = 0;
        int i = 0;
        while (i < 99) {
            Vector3 nextPos = position + impulseBlocksDir[i];

            if (HaveColliderConnect(nextPos)) {
                break;
            }

            impulseBlocks[i] = nextPos;
            impulseBlocksDir[i + 1] = _dir;
            position = nextPos;
            _lastImpulse = i;
            i++;
        }

        if (i == 0) {
            _lastImpulseTemp = -1;
        }

        _impulse = impulseBlocks;
        _impulseDir = impulseBlocksDir;
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
            GameObject blockObj = Instantiate(_impulseObject, _impulse[i], Quaternion.LookRotation(_impulseDir[i]), transform);
            blockObj.transform.right = _impulseDir[i];
            _Impulse[i] = blockObj.GetComponent<ImpulseController>();
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
}
