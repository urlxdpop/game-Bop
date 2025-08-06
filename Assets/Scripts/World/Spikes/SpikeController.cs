using UnityEngine;

[SelectionBase]
public class SpikeController : MonoBehaviour
{
    [SerializeField] private Vector3 _dir;

    private void OnValidate() {
        SetMovingOrientation();
    }
    
    public Vector2 Orientation() {
        return _dir;
    }

    private void SetMovingOrientation() {
        transform.rotation = Quaternion.Euler(0, 0, _dir.y != 0 ? _dir.y * 90 : _dir.x < 0 ? 180 : 0);
    }
}
