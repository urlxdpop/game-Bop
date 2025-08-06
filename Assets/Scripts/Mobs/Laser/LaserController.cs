using UnityEngine;

[SelectionBase]
public class LaserController : MonoBehaviour {
    [SerializeField] private Vector3 _dir;
    [SerializeField] private LayerMask _foreground;

    private Vector3[] _lasers;
    private Vector3[] _lasersDir;

    private void Start() {
        CheckWall();

        Debug.Log(_lasers);
    }

    private void CheckWall() {
        Vector3[] lasers = new Vector3[100];
        Vector3[] lasersDir = new Vector3[100];

        lasersDir[0] = _dir;

        Vector3 position = transform.position;

        int i = 0;
        while (i < 99) {
            Collider2D collider = Physics2D.OverlapCircle(position + lasersDir[i], 0.2f, _foreground);
            if (collider) {
                return;
            }
            lasers[i] = position + lasersDir[i];
            lasersDir[++i] = _dir;

            position += lasersDir[i];
        }

        _lasers = lasers;
        _lasersDir = lasersDir;
    }
}
