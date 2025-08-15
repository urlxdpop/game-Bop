using UnityEngine;

public class ImpulseBlockController : MonoBehaviour {
    [SerializeField] private Vector3 _dir;
    [SerializeField] private LayerMask _foreground;
    [SerializeField] private LayerMask _mobs;
    [SerializeField] private GameObject _impulseBlock;

    private Vector3[] _impulseBlocks;
    private Vector3[] _impulseBlocksDir;
    private GameObject[] _ImpulseBlocks;
    private int _lastImpulseBlock;
    private int _lastImpulseBlockTemp;

    private void OnValidate() {
        SetOrientation(_dir);
    }

    private void Start() {
        _ImpulseBlocks = new GameObject[100];
        //TraceImpulseBlockPath();
        //SpawnImpulseBlocks();
    }

    private void Update() {
        TraceImpulsePath();
        if (_lastImpulseBlockTemp != _lastImpulseBlock) {
            SpawnImpulse();
            _lastImpulseBlockTemp = _lastImpulseBlock;
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

        _lastImpulseBlock = 0;
        int i = 0;
        while (i < 99) {
            Vector3 nextPos = position + impulseBlocksDir[i];
            Collider2D collider = Physics2D.OverlapCircle(nextPos, 0.2f, _foreground);

            if (collider && !IgnoreObject(collider)) {
                break;
            }

            impulseBlocks[i] = nextPos;
            impulseBlocksDir[i + 1] = _dir;
            position = nextPos;
            _lastImpulseBlock = i;
            i++;
        }

        _impulseBlocks = impulseBlocks;
        _impulseBlocksDir = impulseBlocksDir;
    }

    private bool IgnoreObject(Collider2D collider) {
        return collider.GetComponent<IImpulseObject>() != null ||
            collider.GetComponent<ImpulseController>();
    }

    private void SpawnImpulse() {
        RemoveImpulse();

        for (int i = 0; i < _lastImpulseBlock + 1; i++) {
            GameObject blockObj = Instantiate(_impulseBlock, _impulseBlocks[i], Quaternion.LookRotation(_impulseBlocksDir[i]), transform);
            blockObj.transform.right = _impulseBlocksDir[i];
            _ImpulseBlocks[i] = blockObj;
        }
    }

    private void RemoveImpulse() {
        for (int i = 0; i < _ImpulseBlocks.Length; i++) {
            if (_ImpulseBlocks[i] != null) {
                Destroy(_ImpulseBlocks[i]);
                _ImpulseBlocks[i] = null;
            }else {
                return;
            }
        }
    }
}
