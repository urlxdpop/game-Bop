using UnityEngine;

public class MirrorController : MonoBehaviour
{
    [SerializeField] private Vector3 _dir;

    private void OnValidate() {
        SetOrientation();
    }

    public Vector3 GetDir() {
        return _dir;
    }

    private void SetOrientation() {
        transform.rotation = Quaternion.Euler(_dir.y == -1 ? 0 : 180, _dir.x == 1 ? 0 : 180, 0);
    }

    public Vector3 RotateRay(Vector3 dir) {
        if(dir.x != 0 && dir.x != _dir.x) {
            dir = new Vector3(0, _dir.y, 0);
        }else if (dir.y != 0 && dir.y != _dir.y) {
            dir = new Vector3(_dir.x, 0, 0);
        }else{
            dir = Vector3.zero;
        }
        
        return dir.normalized;
    }
}
