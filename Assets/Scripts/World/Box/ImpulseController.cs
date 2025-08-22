using UnityEngine;

public class ImpulseController : MonoBehaviour {
    [SerializeField] private LayerMask _everythingLayer;
    [SerializeField] private float _speed = 0.5f;

    private Vector3 _dir;

    private void Start() {
        CheckObject();
    }

    private void Update() {
        CheckObject();
    }

    public void SetDir(Vector3 dir) {
        _dir = dir.normalized;
    }

    private void CheckObject() {
        Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, 0.2f, _everythingLayer);

        foreach (Collider2D col in collision) {
            IImpulseObject impulseObject = col.GetComponent<IImpulseObject>();

            if (impulseObject != null) {
                ActivatedObj(impulseObject);
            }
        }
        
    }

    private void ActivatedObj(IImpulseObject obj) {
        obj.Impulse(_dir, _speed);
    }
}
