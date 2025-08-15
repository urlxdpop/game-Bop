using UnityEngine;

public class ImpulseController : MonoBehaviour {
    [SerializeField] private LayerMask _everythingLayer;
    [SerializeField] private float _speed = 0.5f;

    private ImpulseBlockController _impulseBlock;

    private void Awake() {
        _impulseBlock = GetComponentInParent<ImpulseBlockController>();
    }

    private void Start() {
        CheckObject();
    }

    private void Update() {
        CheckObject();
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
        obj.Impulse(_impulseBlock.Dir(), _speed);
    }
}
