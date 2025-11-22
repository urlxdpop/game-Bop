using Unity.AppUI.Core;
using UnityEngine;

[SelectionBase]
public class SpikeController : MonoBehaviour
{
    [SerializeField] private Direction _direction;
    private Vector3 _dir;

    private void OnValidate() {
        _dir = SetDir(_direction);
        SetMovingOrientation();
    }

    private void Start() {
        _dir = SetDir(_direction);
        SetMovingOrientation();
    }

    public Vector3 Orientation() {
        return _dir;
    }

    private Vector3 SetDir(Direction dir) {
        return dir switch {
            Direction.UP => Vector3.up,
            Direction.DOWN => Vector3.down,
            Direction.LEFT => Vector3.left,
            Direction.RIGHT => Vector3.right,
            _ => Vector3.zero,
        };
    }

    private void SetMovingOrientation() {
        transform.rotation = Quaternion.Euler(0, 0, _dir.y != 0 ? _dir.y * 90 : _dir.x < 0 ? 180 : 0);
    }
}
